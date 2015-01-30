using Shadowsocks.DomainModel;
using shadowsocks_csharp.util;
using Sweet.LoveWinne.Model;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Shadowsocks.Clients
{
    public class UpdateServerListClient
    {
        private readonly string _host;
        private readonly WebHeaderCollection _webHeaderCollection;
        private string _token;

        public UpdateServerListClient(Configuration config, WebHeaderCollection webHeaderCollection)
        {
            _webHeaderCollection = webHeaderCollection;

            if (config != null && string.IsNullOrEmpty(config.ApiAddress) != true)
            {
                _host = config.ApiAddress;
                if (_host.EndsWith("/"))
                {
                    _host = Regex.Replace(_host, "/+^", "");
                }
            }
            else
            {
                throw new Exception("Need Api Address,Please Check config.db");
            }
        }

        public event EventHandler GetServerListSuccess;

        public RegisterResponse Register(RegisterRequest request)
        {
            var apiAddress = _host + "/Account/Register";

            var result = HttpUtil.Post<RegisterRequest, RegisterResponse>(apiAddress, request, _webHeaderCollection);

            return result;
        }

        public LoginResponse Login(LoginRequest request)
        {
            var apiAddress = _host + "/Account/Login";

            var result = HttpUtil.Post<LoginRequest, LoginResponse>(apiAddress, request, _webHeaderCollection);

            if (result != null && result.IsSuccess)
            {
                _token = result.Token;
            }

            return result;
        }

        public GetServerListResponse GetServerList(GetServerListRequest request)
        {
            var apiAddress = _host + "/ShadowServer/GetServerList";

            var result = HttpUtil.Post<GetServerListRequest, GetServerListResponse>(apiAddress, request, _webHeaderCollection);

            return result;
        }

        public GetQuestionListResponse GetQuestionList(GetQuestionListRequest request)
        {
            var apiAddress = _host + "/Question/GetQuestionList";

            var result = HttpUtil.Post<GetQuestionListRequest, GetQuestionListResponse>(apiAddress, request, _webHeaderCollection);

            return result;
        }

        public AnswertQuestionListResponse AnswertQuestionList(AnswertQuestionListRequest request)
        {
            var apiAddress = _host + "/Question/AnswertQuestionList";

            var result = HttpUtil.Post<AnswertQuestionListRequest, AnswertQuestionListResponse>(apiAddress, request, _webHeaderCollection);

            return result;
        }
    }
}