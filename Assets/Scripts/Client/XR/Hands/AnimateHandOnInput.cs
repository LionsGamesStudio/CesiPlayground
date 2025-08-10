using UnityEngine;
using UnityEngine.InputSystem;

namespace CesiPlayground.Client.XR.Hands
{
    /// <summary>
    /// A client-side script that drives hand animations based on
    /// the continuous values of trigger and grip input actions.
    /// </summary>
    public class AnimateHandOnInput : MonoBehaviour
    {
        [SerializeField] private InputActionProperty pinchAnimationAction;
        [SerializeField] private InputActionProperty gripAnimationAction;
        [SerializeField] private Animator handAnimator;

        private void Update()
        {
            if (handAnimator == null) return;

            float triggerValue = pinchAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Trigger", triggerValue);

            float gripValue = gripAnimationAction.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", gripValue);
        }
    }
}