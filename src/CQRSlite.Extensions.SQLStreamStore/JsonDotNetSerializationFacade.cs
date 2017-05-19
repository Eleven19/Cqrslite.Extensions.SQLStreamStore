using System;
using Newtonsoft.Json;

namespace CQRSlite.Extensions.SQLStreamStore
{
    public class JsonDotNetSerializationFacade : ISerializationFacade
    {
        public JsonDotNetSerializationFacade(JsonSerializerSettings serializerSettings)
        {
            SerializerSettings = serializerSettings;
        }

        public JsonSerializerSettings SerializerSettings { get; }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, SerializerSettings);
        }

        public string Serialize(object value, Type type)
        {
            return JsonConvert.SerializeObject(value, type, SerializerSettings);
        }

        public T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public object Deserialize(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type);
        }

        public static JsonDotNetSerializationFacade Create(JsonSerializerSettings serializerSettings = null) =>
            new JsonDotNetSerializationFacade(serializerSettings);
    }
}