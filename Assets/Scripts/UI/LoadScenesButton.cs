using Assets.Scripts.Core.Scene;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LoadScenesButton : MonoBehaviour
    {
        [SerializeField]
        private List<DataScene> _dataScenes;


        public void OnClick()
        {
            foreach (DataScene dataScene in _dataScenes)
            {
                StartCoroutine(ScenesManager.Instance.InstantiateScene(dataScene));
            }
        }

    }
}