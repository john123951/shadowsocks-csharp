namespace shadowsocks_csharp.model.Response
{
    public abstract class BaseResponse
    {
        public bool IsSuccess { get; set; }

        public string Msg { get; set; }
    }
}