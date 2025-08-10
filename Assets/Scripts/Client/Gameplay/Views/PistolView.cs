using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;
using CesiPlayground.Client.Players.Objects;

namespace CesiPlayground.Client.Gameplay.Views
{
    /// <summary>
    /// The client-side representation of a pistol. It handles grabbing and
    /// firing INTENT, and now correctly reports which hand fired the shot.
    /// </summary>
    public class PistolView : GrabbableObject
    {
        [Header("Effects")]
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject shootEffect;
        [SerializeField] private AudioSource shootSound;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            Grabbable.activated.AddListener(OnFireIntent);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Grabbable.activated.RemoveListener(OnFireIntent);
        }

        /// <summary>
        /// Called when the player activates the pistol (pulls the trigger).
        /// </summary>
        private void OnFireIntent(ActivateEventArgs arg)
        {
            // We can only fire if the pistol is being held.
            if (!IsHeld) return;

            // 1. Play immediate client-side feedback.
            if (shootEffect != null) Instantiate(shootEffect, spawnPoint.position, spawnPoint.rotation);
            if (shootSound != null) shootSound.Play();

            // 2. Raise an event to signal the INTENT to fire, now including the hand!
            GameEventSystem.Client.Raise(new ClientPlayerActionEvent
            {
                ActionType = PlayerActionType.Primary,
                State = InputActionState.Performed,
                Hand = this.CurrentHand
            });
        }
    }
}