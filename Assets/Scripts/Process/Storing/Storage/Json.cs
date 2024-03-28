using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Process.Storing.Storage
{
    public class Json : IStorage
    {
        private JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Converters =
            {

            },
            Formatting = Newtonsoft.Json.Formatting.Indented,
        };

        public T Get<T>(string filepath) where T : class 
        {
            if(!System.IO.File.Exists(filepath))
            {
                return null;
            }

            string json = "";
            using(System.IO.StreamReader stream = new System.IO.StreamReader(filepath))
            {
                string line = stream.ReadLine();
                while(line != null)
                {
                    json += line;
                    line = stream.ReadLine();
                }
            }

            T objectFromJson = JsonConvert.DeserializeObject<T>(json);

            return objectFromJson;
        }

        public T GetFromString<T>(string text) where T : class
        {
            if(string.IsNullOrEmpty(text)) 
                return default(T);

            T objectFromJson = JsonConvert.DeserializeObject<T>(text, settings);

            return objectFromJson;
        }

        public void Store<T>(string filepath, T value) where T : class
        {
            string json = JsonConvert.SerializeObject(value, settings);

            if(!System.IO.File.Exists(filepath))
                System.IO.File.Create(filepath).Close();

            using(System.IO.StreamWriter stream  = new System.IO.StreamWriter(filepath))
            {
                stream.Write(json);
            }
        }
    }
}
