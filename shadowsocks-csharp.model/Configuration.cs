﻿using Shadowsocks.Controller;
using shadowsocks_csharp.util;
using shadowsocks_csharp.util.Security;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shadowsocks.DomainModel
{
    [Serializable]
    public class Configuration
    {
        public List<Server> configs { get; set; }

        public int index { get; set; }

        public bool global { get; set; }

        public bool enabled { get; set; }

        public bool shareOverLan { get; set; }

        public bool isDefault { get; set; }

        public UserInfo UserInfo { get; set; }

        public string ApiAddress { get; set; }

        private static string CONFIG_FILE = "config.db";

        public static string ClientId
        {
            get { return HardwareUtility.GetHardDiskSerialNo(); }
        }

        public Server GetCurrentServer()
        {
            if (index >= 0 && index < configs.Count)
            {
                return configs[index];
            }
            else
            {
                return GetDefaultServer();
            }
        }

        public static void CheckServer(Server server)
        {
            CheckPort(server.local_port);
            CheckPort(server.server_port);
            CheckPassword(server.password);
            CheckServer(server.server);
        }

        public static Configuration Load()
        {
            try
            {
                string configContent = File.ReadAllText(CONFIG_FILE);

                //解密
                var data = AesUtility.DecryptString(configContent, ClientId);

                Configuration config = JsonUtility.Deserialize<Configuration>(data);
                config.isDefault = false;
                return config;
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException))
                {
                    Console.WriteLine(e);
                }
                return new Configuration
                {
                    index = 0,
                    isDefault = true,
                    ApiAddress = "http://api.lovewinne.com/",
                    configs = new List<Server>()
                    {
                        GetDefaultServer()
                    }
                };
            }
        }

        public static void Save(Configuration config)
        {
            if (config.index >= config.configs.Count)
            {
                config.index = config.configs.Count - 1;
            }
            if (config.index < 0)
            {
                config.index = 0;
            }
            config.isDefault = false;
            try
            {
                string jsonString = JsonUtility.Serialize(config);

                //加密
                var data = AesUtility.EncryptString(jsonString, ClientId);

                using (StreamWriter sw = new StreamWriter(File.Open(CONFIG_FILE, FileMode.Create)))
                {
                    sw.Write(data);
                    sw.Flush();
                }
            }
            catch (IOException e)
            {
                Console.Error.WriteLine(e);
            }
        }

        public static Server GetDefaultServer()
        {
            return new Server();
        }

        private static void CheckPort(int port)
        {
            if (port <= 0 || port > 65535)
            {
                throw new ArgumentException(I18N.GetString("Port out of range"));
            }
        }

        private static void CheckPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(I18N.GetString("Password can not be blank"));
            }
        }

        private static void CheckServer(string server)
        {
            if (string.IsNullOrEmpty(server))
            {
                throw new ArgumentException(I18N.GetString("Server IP can not be blank"));
            }
        }
    }
}