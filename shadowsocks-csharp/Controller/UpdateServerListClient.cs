using Shadowsocks.DomainModel;
using shadowsocks_csharp.model.Request;
using shadowsocks_csharp.model.Response;
using System;
using System.Collections.Generic;
using System.Threading;
using Sweet.LoveWinne.Model;

namespace Shadowsocks.Controller
{
    public class UpdateServerListClient
    {
        private readonly string _host;

        public UpdateServerListClient(Configuration config)
        {
            if (config != null && string.IsNullOrEmpty(config.ApiAddress) != true)
            {
                _host = config.ApiAddress;
            }
            else
            {
                throw new Exception("Need Api Address,Please Check config.db");
            }
        }

        public event EventHandler GetServerListSuccess;

        protected virtual void OnGetServerListSuccess()
        {
            EventHandler handler = GetServerListSuccess;
            if (handler != null) handler(this, EventArgs.Empty);
        }


        public RegisterResponse Register(RegisterRequest request)
        {
            return null;
        }

        public LoginResponse Login(LoginRequest request)
        {
            var result = new LoginResponse()
            {
                IsSuccess = true,
                //Notify = "111",
                Message = "222",
                Token = DateTime.Now.ToString()
            };

            Thread.Sleep(3000);

            return result;
        }

        public GetServerListResponse GetServerList(GetServerListRequest request)
        {
            var result = new GetServerListResponse
            {
                IsSuccess = true,
                Message = "2222",
                ServerList =
                    new List<ServerDto>()
                    {
                        new ServerDto()
                        {
                            Server = "107.183.143.200",
                            ServerPort = 1111,
                            LocalPort = 1008,
                            Method = "aes-256-cfb",
                            Password = "demo",
                            Remarks = "test"
                        }
                    }
            };

            Thread.Sleep(3000);

            return result;
        }

        public GetQuestionListResponse GetQuestionList(GetQuestionListRequest request)
        {
            return null;
        }
    }
}