using shadowsocks_csharp.model.Dto;
using System.Collections.Generic;

namespace shadowsocks_csharp.model.Response
{
    public class GetServerListResponse : BaseResponse
    {
        public List<ServerDto> ServerList { get; set; }
    }
}