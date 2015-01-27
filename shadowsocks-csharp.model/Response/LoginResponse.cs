namespace shadowsocks_csharp.model.Response
{
    public class LoginResponse : BaseResponse
    {
        public string Token { get; set; }

        public string Notify { get; set; }
    }
}