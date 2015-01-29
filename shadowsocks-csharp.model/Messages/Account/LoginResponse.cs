using shadowsocks_csharp.model.Response;

namespace Sweet.LoveWinne.Model
{
    public class LoginResponse : BaseResponse
    {
        public string Token { get; set; }

        public string Notify { get; set; }
    }
}