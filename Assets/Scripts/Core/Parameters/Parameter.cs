using Assets.Scripts.Core.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Core.Parameters
{
    public class Parameter : MonoBehaviour
    {
        public Camera XRCamera;

        [Header("Parameters")]
        public GameObject ParametersWindow;

        [Header("Scoreboards")]
        public ScoreBoardManager ScoreBoardManager;

        public void Update()
        {
            ParametersWindow.transform.rotation = XRCamera.transform.rotation;
        }

        public void ToggleParam(InputAction.CallbackContext call)
        {
            ParametersWindow.SetActive(!ParametersWindow.activeSelf);
        }
    }
}
