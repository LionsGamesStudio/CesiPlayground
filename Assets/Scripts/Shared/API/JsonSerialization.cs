using System.Collections.Generic;
using Newtonsoft.Json;
using CesiPlayground.Shared.API.Converters;


namespace CesiPlayground.Shared.API
{
    /// <summary>
    /// A static helper class for serializing and deserializing objects using Newtonsoft.Json.
    /// It centralizes all serialization settings, including custom converters and network settings.
    /// </summary>
    public static class JsonSerialization
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            // This is the setting for polymorphism, crucial for network messages.
            TypeNameHandling = TypeNameHandling.Objects,

            // This is where we register all our custom converters.
            Converters = new List<JsonConverter>
            {
                new PlayerDataConverter(),
                new QuaternionConverter(),
                new Vector3Converter(),
                new Vector2Converter(),
            },

            // This makes the JSON output readable for debugging.
            Formatting = Formatting.Indented
        };

        /// <summary>
        /// Serializes an object to a JSON string using the predefined settings.
        /// </summary>
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        /// <summary>
        /// Deserializes a JSON string to an object of the specified type.
        /// </summary>
        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}