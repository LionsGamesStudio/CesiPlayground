using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CesiPlayground.Core;

namespace CesiPlayground.Client.SceneManagement
{
    /// <summary>
    /// A client-side service for managing additive scene loading and unloading.
    /// This has no equivalent on the server.
    /// </summary>
    public class ScenesManager : Singleton<ScenesManager>
    {
        [SerializeField] 
        private List<DataScene> _scenesToLoadAtStart = new List<DataScene>();
        
        protected override void Awake()
        {
            base.Awake();
            if (this.enabled == false) return;

            ServiceLocator.Register(this);
        }
        
        private void Start()
        {
            foreach (DataScene sceneData in _scenesToLoadAtStart)
            {
                LoadScene(sceneData);
            }
        }

        /// <summary>
        /// Loads a scene additively.
        /// </summary>
        /// <param name="sceneData">The data asset of the scene to load.</param>
        public void LoadScene(DataScene sceneData)
        {
            if (sceneData.IsLoaded) return;
            StartCoroutine(LoadSceneRoutine(sceneData));
        }

        /// <summary>
        /// Unloads an additive scene.
        /// </summary>
        /// <param name="sceneData">The data asset of the scene to unload.</param>
        public void UnloadScene(DataScene sceneData)
        {
            if (!sceneData.IsLoaded) return;
            StartCoroutine(UnloadSceneRoutine(sceneData));
        }

        private IEnumerator LoadSceneRoutine(DataScene sceneData)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneData.SceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            sceneData.IsLoaded = true;
            Debug.Log($"Scene '{sceneData.SceneName}' loaded.");
        }
        
        private IEnumerator UnloadSceneRoutine(DataScene sceneData)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneData.SceneName);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
            sceneData.IsLoaded = false;
            Debug.Log($"Scene '{sceneData.SceneName}' unloaded.");
        }
    }
}