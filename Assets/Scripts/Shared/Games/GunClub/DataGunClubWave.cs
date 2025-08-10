using System.Collections.Generic;
using UnityEngine;

namespace CesiPlayground.Shared.Games.GunClub
{
    [CreateAssetMenu(fileName = "DataGunClubWave", menuName = "Wave/GunClubWave", order = 1)]
    public class DataGunClubWave : ScriptableObject
    {
        [Tooltip("Number of targets to spawn in this wave")] 
        public int NumberOfTargets;
        
        [Tooltip("Cooldown between each spawn")] 
        public float SpawnCooldown;
        
        [Tooltip("Time a target stays alive before disappearing")] 
        public float TargetLifetime;
        
        [Tooltip("List of prefab IDs that can be spawned in this wave")] 
        public List<string> PrefabIds; // We use strings, not direct GameObject references
    }
}