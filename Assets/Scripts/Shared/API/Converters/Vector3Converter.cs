using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CesiPlayground.Shared.API.Converters
{
    /// <summary>
    /// A custom Newtonsoft.Json converter for handling Unity's Vector3 struct.
    /// It ensures that only the x, y, and z components are serialized.
    /// </summary>
    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WriteEndObject();
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return Vector3.zero;
            
            JObject jsonObject = JObject.Load(reader);
            float x = jsonObject["x"]?.Value<float>() ?? 0f;
            float y = jsonObject["y"]?.Value<float>() ?? 0f;
            float z = jsonObject["z"]?.Value<float>() ?? 0f;
            return new Vector3(x, y, z);
        }
    }
}