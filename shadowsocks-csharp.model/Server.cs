using System;
using System.Text;
using System.Text.RegularExpressions;
using Shadowsocks.Controller;

namespace Shadowsocks.DomainModel
{
    [Serializable]
    public class Server
    {
        public string server { set; get; }

        public int server_port { set; get; }

        public int local_port { set; get; }

        public string password { set; get; }

        public string method { set; get; }

        public string remarks { set; get; }

        public string FriendlyName()
        {
            if (string.IsNullOrEmpty(server))
            {
                return I18N.GetString("New server");
            }
            if (string.IsNullOrEmpty(remarks))
            {
                return server + ":" + server_port;
            }
            else
            {
                return remarks + " (" + server + ":" + server_port + ")";
            }
        }

        public Server()
        {
            this.server = "";
            this.server_port = 8388;
            this.local_port = 1080;
            this.method = "aes-256-cfb";
            this.password = "";
            this.remarks = "";
        }

        public Server(string ssURL)
            : this()
        {
            string[] r1 = Regex.Split(ssURL, "ss://", RegexOptions.IgnoreCase);
            string base64 = r1[1].ToString();
            byte[] bytes = null;
            for (var i = 0; i < 3; i++)
            {
                try
                {
                    bytes = System.Convert.FromBase64String(base64);
                }
                catch (FormatException)
                {
                    base64 += "=";
                }
            }
            if (bytes == null)
            {
                throw new FormatException();
            }
            try
            {
                string data = Encoding.UTF8.GetString(bytes);
                int indexLastAt = data.LastIndexOf('@');

                string afterAt = data.Substring(indexLastAt + 1);
                int indexLastColon = afterAt.LastIndexOf(':');
                this.server_port = int.Parse(afterAt.Substring(indexLastColon + 1));
                this.server = afterAt.Substring(0, indexLastColon);

                string beforeAt = data.Substring(0, indexLastAt);
                string[] parts = beforeAt.Split(new[] { ':' });
                this.method = parts[0];
                this.password = parts[1];
            }
            catch (IndexOutOfRangeException)
            {
                throw new FormatException();
            }
        }
    }
}