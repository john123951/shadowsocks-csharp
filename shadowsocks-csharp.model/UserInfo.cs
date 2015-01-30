using System;

namespace Shadowsocks.DomainModel
{
    [Serializable]
    public class UserInfo
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}