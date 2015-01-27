using ServiceStack.Text;

namespace shadowsocks_csharp.util
{
    public static class JsonUtility
    {
        public static string Serialize<T>(T model)
        {
            return JsonSerializer.SerializeToString(model);
        }

        public static T Deserialize<T>(string src)
        {
            return JsonSerializer.DeserializeFromString<T>(src);
        }
    }
}