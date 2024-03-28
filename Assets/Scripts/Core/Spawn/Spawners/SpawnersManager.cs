using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Spawn.Spawners
{
    /// <summary>
    /// Global manager of all spawners in the all the game
    /// </summary>
    public class SpawnersManager : Singleton<SpawnersManager>
    {
        [Tooltip("List to keep track of all spawners")]
        public List<Spawner> Spawners = new List<Spawner>();

        /// <summary>
        /// Add a spawner in the list of all spawners
        /// </summary>
        /// <param name="spawner"></param>
        public void AddSpawner(Spawner spawner)
        {
            Spawners.Add(spawner);
        }

        /// <summary>
        /// Remove a spawner to track
        /// </summary>
        /// <param name="spawner"></param>
        public void RemoveSpawner(Spawner spawner)
        {
            Spawners.Remove(spawner);
        }

        /// <summary>
        /// Remove a spawner to track
        /// </summary>
        /// <param name="idSpawner"></param>
        public void RemoveSpawner(int idSpawner)
        {
            Spawners.Remove(Spawners.Find(x => x.ID == idSpawner));
        }

        public Spawner GetSpawner(int idSpawner)
        {
            return Spawners.Find(x => x.ID == idSpawner);
        }

        
    }
}
