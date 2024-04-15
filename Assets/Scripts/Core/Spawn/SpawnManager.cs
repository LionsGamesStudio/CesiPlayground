using UnityEngine;

namespace Assets.Scripts.Core.Spawn
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        /// <summary>
        /// Instantiate a GameObject
        /// </summary>
        /// <param name="prefab">Prefab to instantiate</param>
        /// <param name="transform">Transform with the position to instantiate the GameObject</param>
        /// <returns></returns>
        public GameObject InstantiateObject(GameObject prefab, Transform transform)
        {
            return Instantiate(prefab, transform.position, Quaternion.identity);
        }

        /// <summary>
        /// Destroy an object
        /// </summary>
        /// <param name="gameObject">GameObject to destroy</param>
        public void DestroyObject(GameObject gameObject)
        {
            if(gameObject == null) return;
            Destroy(gameObject);
        }

        /// <summary>
        /// Destroy a GameObject when a certain time has passed
        /// </summary>
        /// <param name="gameObject">GameObject to destroy</param>
        /// <param name="time">Time to wait before destroying</param>
        public void DestroyObject(GameObject gameObject, float time)
        {
            if(gameObject == null) return;
            Destroy(gameObject, time);
        }
    }
}
