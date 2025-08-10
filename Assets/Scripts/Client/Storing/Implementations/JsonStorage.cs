using System.IO;
using CesiPlayground.Core.Storing;
using CesiPlayground.Shared.API;

namespace CesiPlayground.Client.Storing
{
    /// <summary>
    /// An implementation of IStorage that persists data to local files in JSON format.
    /// It uses the central JsonSerialization helper for all conversion logic.
    /// </summary>
    public class JsonStorage : IStorage
    {
        public T Get<T>(string filepath) where T : class 
        {
            if (!File.Exists(filepath)) return null;

            string json = File.ReadAllText(filepath);
            return JsonSerialization.FromJson<T>(json);
        }

        public T GetFromString<T>(string text) where T : class
        {
            if (string.IsNullOrEmpty(text)) return default(T);
            return JsonSerialization.FromJson<T>(text);
        }

        public string GetFromObject<T>(T obj) where T : class
        {
            return JsonSerialization.ToJson(obj);
        }

        public void Store<T>(string filepath, T value) where T : class
        {
            string json = JsonSerialization.ToJson(value);
            File.WriteAllText(filepath, json);
        }
    }
}