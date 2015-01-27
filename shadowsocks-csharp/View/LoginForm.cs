using Shadowsocks.Controller;
using Shadowsocks.DomainModel;
using shadowsocks_csharp.model.Request;
using System;
using System.Windows.Forms;

namespace Shadowsocks.View
{
    public partial class LoginForm : Form
    {
        public string Token { get; protected set; }

        public UserInfo UserInfo { get; protected set; }

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
            var password = DateTime.Now.ToString();

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show(@"请输入用户名");
                return;
            }

            var loginResponse = _updateServerListClient.Login(new LoginRequest
            {
                ClientId = AppSession.ClientId,
                UserName = userName,
                Password = password
            });

            if (loginResponse.IsSuccess)
            {
                UserInfo = new UserInfo { UserName = userName, Password = password };
                Token = loginResponse.Token;

                if (string.IsNullOrEmpty(loginResponse.Notify) != true)
                {
                    MessageBox.Show(loginResponse.Notify);
                }

                this.DialogResult = DialogResult.OK;

                return;
            }
            else
            {
                var msg = loginResponse.Msg;
                MessageBox.Show(msg);
            }
        }
    }
}