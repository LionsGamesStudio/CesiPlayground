using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Client.SceneManagement;

namespace CesiPlayground.Client.UI
{
    /// <summary>
    /// A client-side button that loads one or more scenes additively.
    /// </summary>
    public class LoadScenesButton : MonoBehaviour
    {
        [SerializeField]
        private List<DataScene> dataScenesToLoad;

        /// <summary>
        /// To be called from a UI Button's OnClick() event.
        /// </summary>
        public void OnClick()
        {
            var scenesManager = ServiceLocator.Get<ScenesManager>();
            if (scenesManager == null)
            {
                Debug.LogError("ScenesManager service not found! Ensure it exists in your scene.");
                return;
            }

            if (dataScenesToLoad == null) return;

            foreach (DataScene dataScene in dataScenesToLoad)
            {
                if (dataScene != null)
                {
                    scenesManager.LoadScene(dataScene);
                }
            }
        }
    }
}