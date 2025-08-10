using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CesiPlayground.Shared.API.Converters
{
    /// <summary>
    /// A custom Newtonsoft.Json converter for handling Unity's Quaternion struct.
    /// It ensures that only the x, y, z, and w components are serialized.
    /// </summary>
    public class QuaternionConverter : JsonConverter<Quaternion>
    {
        public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WritePropertyName("w");
            writer.WriteValue(value.w);
            writer.WriteEndObject();
        }

        public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return Quaternion.identity;

            JObject jsonObject = JObject.Load(reader);
            float x = jsonObject["x"]?.Value<float>() ?? 0f;
            float y = jsonObject["y"]?.Value<float>() ?? 0f;
            float z = jsonObject["z"]?.Value<float>() ?? 0f;
            float w = jsonObject["w"]?.Value<float>() ?? 1f;
            return new Quaternion(x, y, z, w);
        }
    }
}