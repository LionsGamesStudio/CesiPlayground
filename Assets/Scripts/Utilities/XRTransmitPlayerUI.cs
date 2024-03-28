using Assets.Scripts.Core;
using Assets.Scripts.UI;
using Assets.Scripts.UI.GameUI;
using Assets.Scripts.UI.PlayerUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Assets.Scripts.Utilities
{
    [RequireComponent(typeof(XRBaseInteractor))]
    public class XRTransmitPlayerUI : MonoBehaviour
    {
        public XRRayInteractor RayInteractor;
        public Player Player;


        public void Awake()
        {
            if(RayInteractor == null)
                RayInteractor = GetComponent<XRRayInteractor>();

            RayInteractor.uiHoverEntered.AddListener(OnSelectEnterPlayerButton);
            RayInteractor.uiHoverEntered.AddListener(OnSelectChangeNamePlayerUI);
        }

        private void OnSelectEnterPlayerButton(UIHoverEventArgs args)
        {
            StartGameButton button = args.uiObject.GetComponent<StartGameButton>();

            if (button == null)
                button = args.uiObject.GetComponentInParent<StartGameButton>();

            if(button == null)
                button = args.uiObject.GetComponentInChildren<StartGameButton>();

            if (button != null)
            {
                button.Player = Player;
            }
        }

        private void OnSelectChangeNamePlayerUI(UIHoverEventArgs args)
        {
            ChangeNamePlayerUI input = args.uiObject.GetComponent<ChangeNamePlayerUI>();

            if (input == null)
                input = args.uiObject.GetComponentInParent<ChangeNamePlayerUI>();

            if (input == null)
                input = args.uiObject.GetComponentInChildren<ChangeNamePlayerUI>();

            if (input != null)
            {
                input.Player = Player;
            }
        }
    }
}
