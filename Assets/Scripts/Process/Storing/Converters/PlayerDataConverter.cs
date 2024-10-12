using Assets.Scripts.Core.Players;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Process.Storing.Converters
{
    public class PlayerDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PlayerData);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            PlayerData playerData = new PlayerData();

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string propertyName = (string)reader.Value;

                    reader.Read();

                    switch (propertyName)
                    {
                        case "id":
                            playerData.PlayerId = Convert.ToString(reader.Value);
                            break;
                        case "pseudo":
                            playerData.PlayerName = (string)reader.Value;
                            break;
                        case "password":
                            playerData.PlayerPassword = (string)reader.Value;
                            break;
                    }
                }
            }

            return playerData;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PlayerData playerData = (PlayerData)value;

            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(playerData.PlayerId);

            writer.WritePropertyName("pseudo");
            writer.WriteValue(playerData.PlayerName);

            writer.WritePropertyName("password");
            writer.WriteValue(playerData.PlayerPassword);

            writer.WriteEndObject();
        }
    }
}
