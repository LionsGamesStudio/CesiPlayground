using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Process.MollSmash.Game
{
    [CreateAssetMenu(fileName = "new MollSmash Difficulty", menuName = "Difficulty/MollSmashDifficulty", order = 1)]
    public class DataMollSmashDifficulty : ScriptableObject
    {
        public string Name;
        public int LifePoint;
        public List<GameObject> PrefabsMolls;

        [Range(0f, 1f)]
        public float DifficultyMultiplier;

        [Range(0f, 1f)]
        public float MultiplierReducer;

        public AnimationCurve NumberOfEnnemies;

        [Range(0f, 100f)]
        public int GoOutsideRate;
    }
}
