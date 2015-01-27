using shadowsocks_csharp.util;

namespace Shadowsocks.DomainModel
{
    public class AppSession
    {
        private static AppSession _instance;

        private AppSession()
        {
        }

        public static AppSession GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AppSession();
            }

            return _instance;
        }

        private static string _clientId;
        private static string _token;

        public static string ClientId
        {
            get
            {
                if (string.IsNullOrEmpty(_clientId))
                {
                    _clientId = HardwareUtility.GetHardDiskSerialNo();
                }
                return _clientId;
            }
        }

        public static string Token
        {
            get { return _token; }
        }

        public string this[string a]
        {
            get { return null; }
        }
    }
}