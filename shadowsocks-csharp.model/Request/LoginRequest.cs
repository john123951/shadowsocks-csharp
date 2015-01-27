namespace shadowsocks_csharp.model.Request
{
    public class LoginRequest : BaseRequest
    {
        public string ClientId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}