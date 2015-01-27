namespace shadowsocks_csharp.model.Dto
{
    public class ServerDto
    {
        public string Server { set; get; }

        public int ServerPort { set; get; }

        public int LocalPort { set; get; }

        public string Password { set; get; }

        public string Method { set; get; }

        public string Remarks { set; get; }
    }
}