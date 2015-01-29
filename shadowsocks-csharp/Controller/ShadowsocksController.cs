using Shadowsocks.DomainModel;
using Shadowsocks.Mappers;
using Shadowsocks.View;
using shadowsocks_csharp.model.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Sweet.LoveWinne.Model;

namespace Shadowsocks.Controller
{
    public class ShadowsocksController
    {
        // controller:
        // handle user actions
        // manipulates UI
        // interacts with low level logic

        private Thread _ramThread;

        private Local local;
        private PACServer pacServer;
        private Configuration _config;
        private PolipoRunner polipoRunner;
        private GFWListUpdater gfwListUpdater;
        private bool stopped = false;
        private string _token;
        private readonly UpdateServerListClient _updateServerListClient;

        private bool _systemProxyIsDirty = false;

        public bool LoginSuccess
        {
            get
            {
                var result = _config.UserInfo != null && string.IsNullOrEmpty(_token) != true;
                return result;
            }
        }

        public class PathEventArgs : EventArgs
        {
            public string Path;
        }

        public event EventHandler ConfigChanged;

        public event EventHandler EnableStatusChanged;

        public event EventHandler EnableGlobalChanged;

        public event EventHandler ShareOverLANStatusChanged;

        // when user clicked Edit PAC, and PAC file has already created
        public event EventHandler<PathEventArgs> PACFileReadyToOpen;

        public event EventHandler UpdatePACFromGFWListCompleted;

        public event ErrorEventHandler UpdatePACFromGFWListError;

        public event ErrorEventHandler Errored;

        public ShadowsocksController()
        {
            _config = Configuration.Load();
            _updateServerListClient = new UpdateServerListClient(_config);
        }

        public void Start()
        {
            //read remote server list
            LoginAndUpdateServerList(_config);

            Reload();
        }

        protected void ReportError(Exception e)
        {
            if (Errored != null)
            {
                Errored(this, new ErrorEventArgs(e));
            }
        }

        public Server GetCurrentServer()
        {
            return _config.GetCurrentServer();
        }

        // always return copy
        public Configuration GetConfiguration()
        {
            var config = Configuration.Load();

            return config;
        }

        private void LoginAndUpdateServerList(Configuration config)
        {
            try
            {
                var userInfo = config.UserInfo;

                //无配置信息
                if (userInfo == null || string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Password))
                {
                    var regForm = new RegistryForm(_updateServerListClient);

                    regForm.ShowDialog();
                    if (regForm.RegisrySuccess != true)
                    {
                        //注册失败，返回
                        return;
                    }
                }
                if (LoginSuccess != true)
                {
                    //登录
                    var loginSuccess = UserLogin(config.UserInfo);
                    if (loginSuccess != true)
                    {
                        var loginForm = new LoginForm(_updateServerListClient);

                        loginForm.ShowDialog();

                        if (loginForm.LoginSuccess != true)
                        {
                            //登录失败
                            return;
                        }

                        //保存配置
                        config.UserInfo = loginForm.UserInfo;
                        this._token = loginForm.Token;
                    }
                }

                if (LoginSuccess)
                {
                    Configuration.Save(config);
                    UpdateServerList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateServerList()
        {
            try
            {
                var response = _updateServerListClient.GetServerList(new GetServerListRequest
                        {
                            //ClientId = AppSession.ClientId,
                            Token = _token
                        });

                if (response.IsSuccess)
                {
                    var serverList = response.ServerList.ToEntity();
                    SaveServers(serverList);
                }
                else
                {
                    MessageBox.Show(response.Message);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool UserLogin(UserInfo userInfo)
        {
            bool result = false;
            try
            {
                var loginResponse = _updateServerListClient.Login(new LoginRequest
                {
                    //ClientId = AppSession.ClientId,
                    UserName = userInfo.UserName,
                    Password = userInfo.Password
                });
                result = loginResponse.IsSuccess;
                if (loginResponse.IsSuccess)
                {
                    _token = loginResponse.Token;
                    if (string.IsNullOrEmpty(loginResponse.Notify) != true)
                    {
                        MessageBox.Show(loginResponse.Notify);
                    }
                }
                else
                {
                    var msg = loginResponse.Message;
                    MessageBox.Show(msg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
        }

        public void SaveServers(List<Server> servers)
        {
            _config.configs = servers;
            SaveConfig(_config);
        }

        public bool AddServerBySSURL(string ssURL)
        {
            try
            {
                var server = new Server(ssURL);
                _config.configs.Add(server);
                _config.index = _config.configs.Count - 1;
                SaveConfig(_config);
                return true;
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
                return false;
            }
        }

        public void ToggleEnable(bool enabled)
        {
            _config.enabled = enabled;
            UpdateSystemProxy();
            SaveConfig(_config);
            if (EnableStatusChanged != null)
            {
                EnableStatusChanged(this, new EventArgs());
            }
        }

        public void ToggleGlobal(bool global)
        {
            _config.global = global;
            UpdateSystemProxy();
            SaveConfig(_config);
            if (EnableGlobalChanged != null)
            {
                EnableGlobalChanged(this, new EventArgs());
            }
        }

        public void ToggleShareOverLAN(bool enabled)
        {
            _config.shareOverLan = enabled;
            SaveConfig(_config);
            if (ShareOverLANStatusChanged != null)
            {
                ShareOverLANStatusChanged(this, new EventArgs());
            }
        }

        public void SelectServerIndex(int index)
        {
            _config.index = index;
            SaveConfig(_config);
        }

        public void Stop()
        {
            if (stopped)
            {
                return;
            }
            stopped = true;
            if (local != null)
            {
                local.Stop();
            }
            if (polipoRunner != null)
            {
                polipoRunner.Stop();
            }
            if (_config.enabled)
            {
                SystemProxy.Disable();
            }
        }

        public void TouchPACFile()
        {
            string pacFilename = pacServer.TouchPACFile();
            if (PACFileReadyToOpen != null)
            {
                PACFileReadyToOpen(this, new PathEventArgs() { Path = pacFilename });
            }
        }

        public string GetQRCodeForCurrentServer()
        {
            Server server = GetCurrentServer();
            string parts = server.method + ":" + server.password + "@" + server.server + ":" + server.server_port;
            string base64 = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(parts));
            return "ss://" + base64;
        }

        public void UpdatePACFromGFWList()
        {
            if (gfwListUpdater != null)
            {
                gfwListUpdater.UpdatePACFromGFWList();
            }
        }

        protected void Reload()
        {
            // some logic in configuration updated the config when saving, we need to read it again
            _config = Configuration.Load();

            if (polipoRunner == null)
            {
                polipoRunner = new PolipoRunner();
            }
            if (pacServer == null)
            {
                pacServer = new PACServer();
                pacServer.PACFileChanged += pacServer_PACFileChanged;
            }
            if (gfwListUpdater == null)
            {
                gfwListUpdater = new GFWListUpdater();
                gfwListUpdater.UpdateCompleted += pacServer_PACUpdateCompleted;
                gfwListUpdater.Error += pacServer_PACUpdateError;
            }

            pacServer.Stop();

            if (local != null)
            {
                local.Stop();
            }

            // don't put polipoRunner.Start() before pacServer.Stop()
            // or bind will fail when switching bind address from 0.0.0.0 to 127.0.0.1
            // though UseShellExecute is set to true now
            // http://stackoverflow.com/questions/10235093/socket-doesnt-close-after-application-exits-if-a-launched-process-is-open
            polipoRunner.Stop();
            try
            {
                polipoRunner.Start(_config);

                local = new Local(_config);
                local.Start();
                pacServer.Start(_config);
            }
            catch (Exception e)
            {
                // translate Microsoft language into human language
                // i.e. An attempt was made to access a socket in a way forbidden by its access permissions => Port already in use
                if (e is SocketException)
                {
                    SocketException se = (SocketException)e;
                    if (se.SocketErrorCode == SocketError.AccessDenied)
                    {
                        e = new Exception(I18N.GetString("Port already in use"), e);
                    }
                }
                Logging.LogUsefulException(e);
                ReportError(e);
            }

            if (ConfigChanged != null)
            {
                ConfigChanged(this, new EventArgs());
            }

            UpdateSystemProxy();
            Util.Utils.ReleaseMemory();
        }

        protected void SaveConfig(Configuration newConfig)
        {
            Configuration.Save(newConfig);
            Reload();
        }

        private void UpdateSystemProxy()
        {
            if (_config.enabled)
            {
                SystemProxy.Enable(_config.global);
                _systemProxyIsDirty = true;
            }
            else
            {
                // only switch it off if we have switched it on
                if (_systemProxyIsDirty)
                {
                    SystemProxy.Disable();
                    _systemProxyIsDirty = false;
                }
            }
        }

        private void pacServer_PACFileChanged(object sender, EventArgs e)
        {
            UpdateSystemProxy();
        }

        private void pacServer_PACUpdateCompleted(object sender, EventArgs e)
        {
            if (UpdatePACFromGFWListCompleted != null)
                UpdatePACFromGFWListCompleted(this, e);
        }

        private void pacServer_PACUpdateError(object sender, ErrorEventArgs e)
        {
            if (UpdatePACFromGFWListError != null)
                UpdatePACFromGFWListError(this, e);
        }

        private void StartReleasingMemory()
        {
            _ramThread = new Thread(new ThreadStart(ReleaseMemory));
            _ramThread.IsBackground = true;
            _ramThread.Start();
        }

        private void ReleaseMemory()
        {
            while (true)
            {
                Util.Utils.ReleaseMemory();
                Thread.Sleep(30 * 1000);
            }
        }
    }
}