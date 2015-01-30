using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Shadowsocks.Controller
{
    public class I18N
    {
        protected static Dictionary<string, string> Strings;

        public static void Register(string src)
        {
            Strings = new Dictionary<string, string>();

            if (System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag.ToLowerInvariant().StartsWith("zh"))
            {
                string[] lines = Regex.Split(src, "\r\n|\r|\n");
                foreach (string line in lines)
                {
                    string[] kv = Regex.Split(line, "=");
                    if (kv.Length == 2)
                    {
                        Strings[kv[0]] = kv[1];
                    }
                }
            }
        }

        public static string GetString(string key)
        {
            if (Strings.ContainsKey(key))
            {
                return Strings[key];
            }
            else
            {
                return key;
            }
        }
    }
}