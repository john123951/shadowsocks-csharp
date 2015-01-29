using Shadowsocks.Clients;
using Shadowsocks.Controller;
using Shadowsocks.DomainModel;
using shadowsocks_csharp.model.Request;
using System.Windows.Forms;
using Sweet.LoveWinne.Model;

namespace Shadowsocks.View
{
    public partial class LoginForm : Form
    {
        public string Token { get; protected set; }

        public UserInfo UserInfo { get; protected set; }

        public bool LoginSuccess { get; set; }

        private readonly UpdateServerListClient _updateServerListClient;

        protected LoginForm()
        {
            InitializeComponent();
        }

        public LoginForm(UpdateServerListClient updateServerListClient)
            : this()
        {
            _updateServerListClient = updateServerListClient;
        }

        private void btn_login_Click(object sender, System.EventArgs e)
        {
            var userName = txt_userName.Text;
            var password = txt_password.Text;

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show(@"请输入用户名");
                return;
            }

            var loginResponse = _updateServerListClient.Login(new LoginRequest
            {
                //ClientId = AppSession.ClientId,
                UserName = userName,
                Password = password
            });

            LoginSuccess = loginResponse.IsSuccess;
            if (loginResponse.IsSuccess)
            {
                UserInfo = new UserInfo { UserName = userName, Password = password };
                Token = loginResponse.Token;

                if (string.IsNullOrEmpty(loginResponse.Notify) != true)
                {
                    MessageBox.Show(loginResponse.Notify);
                }
                return;
            }
            else
            {
                var msg = loginResponse.Message;
                MessageBox.Show(msg);
            }
        }
    }
}