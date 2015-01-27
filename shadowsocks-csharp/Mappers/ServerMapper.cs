using Shadowsocks.DomainModel;
using shadowsocks_csharp.model.Dto;
using System.Collections.Generic;

namespace Shadowsocks.Mappers
{
    public static class ServerMapper
    {
        public static List<Server> ToEntity(this List<ServerDto> dtos)
        {
            var result = new List<Server>();

            if (dtos == null || dtos.Count <= 0)
            {
                return result;
            }

            foreach (var serverDto in dtos)
            {
                var entity = serverDto.ToEntity();

                result.Add(entity);
            }

            return result;
        }

        public static Server ToEntity(this ServerDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = new Server
            {
                server = dto.Server,
                server_port = dto.ServerPort,
                local_port = dto.LocalPort,
                password = dto.Password,
                method = dto.Method,
                remarks = dto.Remarks
            };

            return entity;
        }
    }
}