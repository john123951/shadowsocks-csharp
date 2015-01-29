using shadowsocks_csharp.model.Request;

namespace Sweet.LoveWinne.Model
{
    public class LoginRequest : BaseRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}