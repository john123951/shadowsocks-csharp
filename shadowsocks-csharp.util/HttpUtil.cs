using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace shadowsocks_csharp.util
{
    /// <summary>
    /// Http通用类
    /// </summary>
    public static class HttpUtil
    {
        #region 公有方法

        /// <summary>
        /// 发送一个HttpPost请求
        /// </summary>
        /// <param name="address">远程地址</param>
        /// <param name="data">需要发送的数据</param>
        /// <returns></returns>

        public static TResponse Post<TRequest, TResponse>(string address, TRequest request)
        {
            var data = JsonUtility.Serialize(request);

            var response = Post(address, data, Encoding.UTF8);

            var result = JsonUtility.Deserialize<TResponse>(response);

            var type = typeof(TResponse);
            if (type.IsValueType != true)
            {
                if (result == null) { result = Activator.CreateInstance<TResponse>(); }
            }

            return result;
        }

        public static string Post(string address, string data)
        {
            return Post(address, data, Encoding.UTF8);
        }

        /// <summary>
        /// 发送一个HttpPost请求
        /// </summary>
        /// <param name="address">远程地址</param>
        /// <param name="data">需要发送的数据</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string Post(string address, string data, Encoding encoding)
        {
            var request = WebRequest.Create(address) as HttpWebRequest;
            Debug.Assert(request != null, "request != null");

            PackRequest(request);
            request.Method = "POST";

            byte[] byteArray = encoding.GetBytes(data);
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            return GetResponse(request);
        }

        /// <summary>
        /// 发送一个HttpGet请求
        /// </summary>
        /// <param name="address">远程地址</param>
        /// <param name="modify">供修改的回调函数</param>
        /// <returns></returns>
        public static string Get(string address, Action<HttpWebRequest> modify = null)
        {
            //兼容Https
            ServicePointManager.ServerCertificateValidationCallback =
                (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

            var request = WebRequest.Create(address) as HttpWebRequest;
            Debug.Assert(request != null, "request != null");

            PackRequest(request);
            request.Method = "GET";

            if (modify != null)
            {
                modify(request);
            }

            return GetResponse(request);
        }

        /// <summary>
        /// 异步下载文件
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="saveFileName"></param>
        /// <param name="userToken"></param>
        /// <param name="downloadProgressChanged"></param>
        /// <param name="downloadFileCompleted"></param>
        public static void DownloadFileAsync(Uri uri, string saveFileName, object userToken = null,
                                                    Action<object, DownloadProgressChangedEventArgs> downloadProgressChanged = null,
                                                    Action<object, AsyncCompletedEventArgs> downloadFileCompleted = null)
        {
            WebClient webClient = new WebClient();

            #region 为webClient添加事件

            if (downloadProgressChanged != null)
            {
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloadProgressChanged);
            }
            if (downloadFileCompleted != null)
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadFileCompleted);
            }

            webClient.DownloadFileCompleted += (o, e) => webClient.Dispose();

            #endregion 为webClient添加事件

            //开始下载
            webClient.DownloadFileAsync(uri, saveFileName, userToken);
        }

        #endregion 公有方法

        #region 私有方法

        /// <summary>
        /// 包装Http Request
        /// </summary>
        /// <param name="request"></param>
        private static void PackRequest(HttpWebRequest request)
        {
            request.ContentType = "application/json;charset=UTF-8";
            request.Accept = "application/json";
        }

        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="request">需要发送的请求</param>
        /// <returns></returns>
        private static string GetResponse(HttpWebRequest request)
        {
            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        #endregion 私有方法
    }
}