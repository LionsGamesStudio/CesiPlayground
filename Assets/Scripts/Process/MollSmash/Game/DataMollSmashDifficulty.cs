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

        [Range(0f, 1f), Tooltip("Multiplier to make the game more or less difficult (the more it is low, the more it is difficult)")]
        public float DifficultyMultiplier;

        [Range(0f, 1f), Tooltip("The amont to decrease the multiplier to make it more difficult as the game go on")]
        public float MultiplierReducer;

        [Tooltip("Curve describing the amount of enemy to spawn through the game as the time pass")]
        public AnimationCurve NumberOfEnnemies;

        [Range(0f, 100f), Tooltip("The rate of moll going outside")]
        public int GoOutsideRate;
    }
}
