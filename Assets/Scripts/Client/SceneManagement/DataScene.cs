using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CesiPlayground.Client.SceneManagement
{
    /// <summary>
    /// A ScriptableObject representing a scene that can be loaded/unloaded by the ScenesManager.
    /// </summary>
    [CreateAssetMenu(fileName = "DataScene", menuName = "ScriptableObjects/DataScene", order = 1)]
    public class DataScene : ScriptableObject
    {
        public string SceneName;
        
        [System.NonSerialized]
        public bool IsLoaded = false; // Runtime state should not be serialized

#if UNITY_EDITOR
        // This logic is good for ensuring a clean state between editor plays.
        private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                IsLoaded = false;
            }
        }
#endif
    }
}