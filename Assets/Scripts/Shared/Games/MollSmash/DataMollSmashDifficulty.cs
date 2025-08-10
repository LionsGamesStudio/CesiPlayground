using System.Collections.Generic;
using UnityEngine;

namespace CesiPlayground.Shared.Games.MollSmash
{
    [CreateAssetMenu(fileName = "DataMollSmashDifficulty", menuName = "Difficulty/MollSmashDifficulty", order = 1)]
    public class DataMollSmashDifficulty : ScriptableObject
    {
         public string Name;
        public int LifePoint;
        public List<string> MollPrefabIds; // We use strings, not GameObjects
        public float SpawnRate; // How many seconds between spawns
        public float DifficultyMultiplier; // General multiplier for speed, etc.
        public float MollLifetime; // How long a moll stays up before escaping
    }
}