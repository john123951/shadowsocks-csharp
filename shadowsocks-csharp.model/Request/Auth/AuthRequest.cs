namespace shadowsocks_csharp.model.Request.Auth
{
    public abstract class AuthRequest : BaseRequest
    {
        public string ClientId { get; set; }

        public string Token { get; set; }
    }
}