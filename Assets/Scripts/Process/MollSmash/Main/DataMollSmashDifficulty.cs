using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Process.MollSmash.Main
{
    [CreateAssetMenu(fileName = "new MollSmash Difficulty", menuName = "Difficulty/MollSmashDifficulty", order = 1)]
    public class DataMollSmashDifficulty : ScriptableObject
    {
        public string Name;
        public int LifePoint;
        public List<GameObject> PrefabsMolls;
        public float SpawnRate;
        public float DifficultyMultiplier;
    }
}
