using shadowsocks_csharp.model.Request;

namespace Sweet.LoveWinne.Model
{
    public class RegisterRequest : BaseRequest
    {
        public string UserName { set; get; }

        public string Password { set; get; }
    }
}