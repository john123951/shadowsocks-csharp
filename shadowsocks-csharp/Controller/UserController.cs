using Shadowsocks.Clients;
using Shadowsocks.DomainModel;
using Shadowsocks.View;
using Sweet.LoveWinne.Model;

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

            if (regResponse.IsSuccess != true)
            {
                return false;
            }

            var listResponse = _updateServerListClient.GetQuestionList(new GetQuestionListRequest());

            if (listResponse.IsSuccess)
            {
                var validateDlg = new ValidateForm(listResponse.QuestionList);

                validateDlg.ShowDialog();

                return validateDlg.IsValidated;
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





            //return false;
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }
    }
}