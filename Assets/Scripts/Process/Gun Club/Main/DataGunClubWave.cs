using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Process.GunClub.Main
{
    [CreateAssetMenu(fileName = "new Gun Club Wave", menuName = "Wave/GunClubWave", order = 1)]
    public class DataGunClubWave : ScriptableObject
    {
        [Tooltip("Number of target to spawn")] public int NumberOfTarget;
        [Tooltip("Cooldown between spawn")] public float CooldownSpawn;
        [Tooltip("Time target alive")] public int CooldownAlive;
        [Tooltip("Prefabs of the targets possibles to spawn")] public List<GameObject> Prefab;

    }
}