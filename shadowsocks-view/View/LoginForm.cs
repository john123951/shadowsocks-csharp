using Shadowsocks.DomainModel;
using System.Windows.Forms;

namespace Shadowsocks.View
{
    public partial class LoginForm : Form
    {
        public UserInfo UserInfo { get; protected set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, System.EventArgs e)
        {
            var userName = txt_userName.Text;
            var password = txt_password.Text;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(@"请输入用户名及密码");
                return;
            }

            UserInfo = new UserInfo
            {
                UserName = userName,
                Password = password
            };

            this.DialogResult = DialogResult.OK;
        }
    }
}