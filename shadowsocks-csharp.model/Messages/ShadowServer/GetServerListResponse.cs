using System.Collections.Generic;
using shadowsocks_csharp.model.Response;

namespace Sweet.LoveWinne.Model
{
    public class GetServerListResponse : BaseResponse
    {
        public List<ServerDto> ServerList { get; set; }
    }
}