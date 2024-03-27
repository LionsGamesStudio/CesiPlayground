using Assets.Scripts.Core.Events;
using Assets.Scripts.Process.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Spawn.Spawners
{
    /// <summary>
    /// A classic spawner
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        [SerializeField] protected List<Transform> _spawnPossibilities = new List<Transform>();
        private List<Transform> _spawnPossibilitiesNotUsed = new List<Transform>();
        private Dictionary<Transform, GameObject> _spawnPossibilitiesAlreadyUsed = new Dictionary<Transform, GameObject>();

        // Generate a unique seed for each spawner id
        private int _id = Guid.NewGuid().GetHashCode();

        private EventBinding<OnDispawnRequestSend> _dispawnSendEventBinding;

        private void Awake()
        {
            if (_spawnPossibilities.Count == 0) Debug.LogError("No location for random position provide.");

            // Add id as name
            this.name = "Spawner_" + _id;

            SpawnersManager.Instance.AddSpawner(this);

            foreach(Transform t in _spawnPossibilities)
            {
                _spawnPossibilitiesNotUsed.Add(t);
            }

            // Events
            _dispawnSendEventBinding = new EventBinding<OnDispawnRequestSend>(ManageDispawn);
            EventBus<OnDispawnRequestSend>.Register(_dispawnSendEventBinding);
        }

        #region Spawning

        /// <summary>
        /// Spawn an entity to a free position 
        /// </summary>
        /// <typeparam name="T">Type of the entity to spawn</typeparam>
        /// <param name="posIndex">Index of the position to use</param>
        /// <param name="entity">Entity to spawn</param>
        /// <returns></returns>
        public virtual GameObject Spawn<T>(int posIndex, T entity)
        {
            if(posIndex < 0 || posIndex > _spawnPossibilitiesNotUsed.Count) return null;

            Transform pos = _spawnPossibilitiesNotUsed[posIndex];
            GameObject obj = SpawnManager.Instance.InstantiateObject(entity as GameObject, pos);

            _spawnPossibilitiesAlreadyUsed.Add(pos, obj);
            _spawnPossibilitiesNotUsed.Remove(pos);

            return obj;
        }

        /// <summary>
        /// Dispawn an entity
        /// </summary>
        /// <param name="entity">Entity to dispawn</param>
        public virtual void Dispawn(GameObject entity)
        {
            if(entity == null) return;
            if(!_spawnPossibilitiesAlreadyUsed.ContainsValue(entity)) return;

            Transform t = null;
            foreach(KeyValuePair<Transform, GameObject> keyValuePair in _spawnPossibilitiesAlreadyUsed)
            {
                if(keyValuePair.Value == entity)
                {
                    t = keyValuePair.Key;
                }
            }

            if(t == null) return;

            _spawnPossibilitiesAlreadyUsed.Remove(t);
            _spawnPossibilitiesNotUsed.Add(t);
            SpawnManager.Instance.DestroyObject(entity);
        }

        /// <summary>
        /// Dispawn an entity after a certain amount of time
        /// </summary>
        /// <param name="entity">Entity to dispawn</param>
        /// <param name="time">Time to wait before dispawning entity</param>
        /// <returns></returns>
        public virtual void Dispawn(GameObject entity, float time)
        {
            if (entity == null) return;
            if (!_spawnPossibilitiesAlreadyUsed.ContainsValue(entity)) return;

            Transform t = null;
            foreach (KeyValuePair<Transform, GameObject> keyValuePair in _spawnPossibilitiesAlreadyUsed)
            {
                if (keyValuePair.Value == entity)
                {
                    t = keyValuePair.Key;
                }
            }

            if (t == null) return;

            SpawnManager.Instance.DestroyObject(entity, time);
            StartCoroutine(WaitForTimeToFreePosition(time, t));
        }


        #endregion

        /// <summary>
        /// Clear the spawner and dispawn all object
        /// </summary>
        public void ClearSpawner()
        {
            foreach(KeyValuePair<Transform, GameObject> keyValuePair in _spawnPossibilitiesAlreadyUsed)
            {
                Destroy(keyValuePair.Value);
            }
            _spawnPossibilitiesAlreadyUsed.Clear();
            _spawnPossibilitiesNotUsed.Clear();

            foreach(Transform t in _spawnPossibilities)
            {
                _spawnPossibilitiesNotUsed.Add(t);
            }
        }

        /// <summary>
        /// Add a delay before freeing a position of spawning
        /// </summary>
        /// <param name="time">Delay to wait for</param>
        /// <param name="position">Position to free</param>
        /// <returns></returns>
        private IEnumerator WaitForTimeToFreePosition(float time, Transform position)
        {
            yield return new WaitForSeconds(time);
            if(_spawnPossibilitiesAlreadyUsed.ContainsKey(position))
            {
                _spawnPossibilitiesAlreadyUsed.Remove(position);
                _spawnPossibilitiesNotUsed.Add(position);
            }
        }

        /// <summary>
        /// Manage how to dispawn an object
        /// </summary>
        /// <param name="e"></param>
        private void ManageDispawn(OnDispawnRequestSend e)
        {
            if (e.Spawner.ID != this.ID) return;

            if(e.TimeBeforeDispawn != 0)
            {
                Dispawn(e.ObjectToDispawn, e.TimeBeforeDispawn);
            }
            else
            {
                Dispawn(e.ObjectToDispawn);
            }
        }

        /// --------- Assure that the spawner is at the right time in the list of spawners ---------   

        protected virtual void OnDisable()
        {
            SpawnersManager.Instance.RemoveSpawner(this);
        }

        protected virtual void OnEnable()
        {
            SpawnersManager.Instance.AddSpawner(this);
        }

        protected virtual void OnDestroy()
        {
            SpawnersManager.Instance.RemoveSpawner(this);
        }

        public int ID { get => _id;}
        public int NumberOfSpawningSpot { get => _spawnPossibilities.Count; }

        public List<Transform> PositionsAvailable { get => _spawnPossibilitiesNotUsed; }
        public Dictionary<Transform, GameObject> SpawnPosibilitiesAlreadyUsed { get => _spawnPossibilitiesAlreadyUsed; }
    }
}
