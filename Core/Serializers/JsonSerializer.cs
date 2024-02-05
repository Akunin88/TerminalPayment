using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;

namespace Core.Serializers
{
    public static class JsonSerializer
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            Formatting = Formatting.Indented
        };

        public static string Serialize<T>(this T obj) => JsonConvert.SerializeObject(obj, settings);

        public static T DeserializeFromContent<T>(string jsonString)
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(jsonString, settings);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error with deserialization from content {jsonString} : {ex.Message}");
            }
        }

        public static void Serialize<T>(this T obj, string fName)
        {
            if (obj == null)
            {
                throw new NullReferenceException($"obj == null. Serialization to file {fName}");
            }
            try
            {
                string jsonString = obj.Serialize();
                File.WriteAllText(fName, jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error with serialization to file {fName} : {ex.Message}");
            }
        }

        public static void Serialize<T>(this T obj, Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();
                ser.Serialize(jsonWriter, obj);
                jsonWriter.Flush();
            }
        }

        public static T Deserialize<T>(Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();
                return ser.Deserialize<T>(jsonReader);
            }
        }

        public static T Deserialize<T>(byte[] data) where T : class
        {
            using (var stream = new MemoryStream(data))
                return Deserialize<T>(stream);
        }

        public static T Deserialize<T>(string path)
        {
            try
            {
                string jsonString = File.ReadAllText(path);
                T obj = JsonConvert.DeserializeObject<T>(jsonString, settings);
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error with deserialization from file {path} : {ex.Message}");
            }
        }
    }
}