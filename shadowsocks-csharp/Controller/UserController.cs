using Shadowsocks.Clients;
using Shadowsocks.DomainModel;
using Shadowsocks.View;
using Sweet.LoveWinne.Model;
using System;
using System.Windows.Forms;

namespace Shadowsocks.Controller
{
    public class UserController
    {
        private readonly UpdateServerListClient _updateServerListClient;

        public UserController(Configuration config)
        {
            _updateServerListClient = new UpdateServerListClient(config);
        }

        public bool Register(UserInfo userInfo)
        {
            if (userInfo == null || string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Password))
            {
                return false;
            }

            var regResponse = _updateServerListClient.Register(new RegisterRequest
             {
                 UserName = userInfo.UserName,
                 Password = userInfo.Password
             });

            if (regResponse.IsSuccess)
            {
                var result = ValidateUser(userInfo);

                return result;
            }

            return false;
        }

        public bool Login(UserInfo userInfo)
        {
            var loginResponse = _updateServerListClient.Login(new LoginRequest
            {
                UserName = userInfo.UserName,
                Password = userInfo.Password
            });

            if (loginResponse.IsSuccess)
            {
                return true;
            }

            //用户密码错误
            if (string.Compare("login_000001", loginResponse.MessageCode, StringComparison.OrdinalIgnoreCase) == 0)
            {
                var loginDlg = new LoginForm();

                if (loginDlg.DialogResult == DialogResult.OK)
                {
                    var result = Login(loginDlg.UserInfo);
                    return result;
                }
            }

            //用户未授权
            if (string.Compare("login_000001", loginResponse.MessageCode, StringComparison.OrdinalIgnoreCase) == 0)
            {
                var result = ValidateUser(userInfo);

                return result;
            }

            return false;
        }

        private bool ValidateUser(UserInfo userInfo)
        {
            var listResponse = _updateServerListClient.GetQuestionList(new GetQuestionListRequest());

            if (listResponse.IsSuccess)
            {
                //提示用户回答问题
                var validateDlg = new ValidateForm(listResponse);

                validateDlg.ShowDialog();

                if (validateDlg.DialogResult == DialogResult.OK)
                {
                    //验证回答
                    var answerQuestionItems = validateDlg.AnswerQuestionItems;

                    var validateResponse = _updateServerListClient.AnswertQuestionList(new AnswertQuestionListRequest
                    {
                        AnswerQuestionList = answerQuestionItems
                    });

                    if (validateResponse.IsSuccess)
                    {
                        //登录
                        var result = Login(userInfo);
                        return result;
                    }
                    else
                    {
                        MessageBox.Show(validateResponse.Message);
                    }
                }
            }

            return false;
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }
    }
}