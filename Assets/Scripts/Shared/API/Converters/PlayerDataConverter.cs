using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CesiPlayground.Shared.Data.Players;

namespace CesiPlayground.Shared.API.Converters
{
    /// <summary>
    /// Custom JSON converter for the PlayerData class.
    /// Ensures consistent serialization/deserialization with the backend API.
    /// </summary>
    public class PlayerDataConverter : JsonConverter<PlayerData>
    {
        public override PlayerData ReadJson(JsonReader reader, Type objectType, PlayerData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            
            JObject jsonObject = JObject.Load(reader);
            var playerData = new PlayerData
            {
                PlayerId = jsonObject["playerId"]?.Value<string>(),
                PlayerName = jsonObject["playerName"]?.Value<string>()
            };
            return playerData;
        }

        public override void WriteJson(JsonWriter writer, PlayerData value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("playerId");
            writer.WriteValue(value.PlayerId);

            writer.WritePropertyName("playerName");
            writer.WriteValue(value.PlayerName);
            
            writer.WriteEndObject();
        }
    }
}