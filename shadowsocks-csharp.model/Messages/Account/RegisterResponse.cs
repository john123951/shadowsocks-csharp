using shadowsocks_csharp.model.Response;

namespace Sweet.LoveWinne.Model
{
    public class RegisterResponse : BaseResponse
    {
        public string Token { get; set; }

        public string Notify { get; set; }
    }
}