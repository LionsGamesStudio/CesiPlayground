using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CesiPlayground.Shared.API.Converters
{
    /// <summary>
    /// A custom Newtonsoft.Json converter for handling Unity's Vector2 struct.
    /// It ensures that only the x and y components are serialized.
    /// </summary>
    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WriteEndObject();
        }

        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return Vector2.zero;

            JObject jsonObject = JObject.Load(reader);
            float x = jsonObject["x"]?.Value<float>() ?? 0f;
            float y = jsonObject["y"]?.Value<float>() ?? 0f;
            return new Vector2(x, y);
        }
    }
}