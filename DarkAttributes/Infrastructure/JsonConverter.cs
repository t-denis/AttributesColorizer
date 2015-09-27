using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DarkAttributes.Infrastructure
{
    public static class JsonConverter
    {
        public static string ToJson<T>(T obj) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        public static T FromJson<T>(string json) where T : class
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
