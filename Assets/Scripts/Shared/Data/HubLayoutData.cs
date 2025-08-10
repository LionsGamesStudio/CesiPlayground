using System.Collections.Generic;
using UnityEngine;

namespace CesiPlayground.Shared.Data.HubLayoutData
{
    [System.Serializable]
    public struct StandConfiguration
    {
        public string GameDataId; // The ID from the DataGame asset
        public Vector3 StandPosition;
        public List<Vector3> SpawnPoints;
    }
    
    [CreateAssetMenu(fileName = "HubLayoutData", menuName = "Server/Hub Layout Data")]
    public class HubLayoutData : ScriptableObject
    {
        public List<StandConfiguration> StandConfigurations = new List<StandConfiguration>();
    }
}