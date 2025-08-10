using System.Collections.Generic;
using UnityEngine;

namespace CesiPlayground.Shared.Data
{
    [CreateAssetMenu(fileName = "NewSpawnPointData", menuName = "Server/Spawn Point Data")]
    public class SpawnPointData : ScriptableObject
    {
        public string GameId;
        public List<Vector3> SpawnPositions = new List<Vector3>();
    }
}