using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Players
{
    [Serializable]
    public class PlayerData
    {
        public string PlayerId;
        public string PlayerName;

        [JsonIgnore]
        public string PlayerPassword;
    }
}
