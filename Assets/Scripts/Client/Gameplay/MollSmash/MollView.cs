using UnityEngine;
using CesiPlayground.Client.Network;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.MollSmash;

namespace CesiPlayground.Client.Gameplay.MollSmash
{
    /// <summary>
    /// The client-side visual representation of a Moll.
    /// It handles animations and effects based on server commands.
    /// </summary>
    [RequireComponent(typeof(NetworkedObjectView), typeof(Animator))]
    public class MollView : MonoBehaviour
    {
        private NetworkedObjectView _networkView;
        private Animator _animator;

        private void Awake()
        {
            _networkView = GetComponent<NetworkedObjectView>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            GameEventSystem.Client.Register<MollStateChangedEvent>(OnMollStateChanged);
        }

        private void OnDisable()
        {
            GameEventSystem.Client.Unregister<MollStateChangedEvent>(OnMollStateChanged);
        }

        /// <summary>
        /// Listens for state changes for THIS moll and triggers animations.
        /// </summary>
        private void OnMollStateChanged(MollStateChangedEvent e)
        {
            if (e.NetworkId != _networkView.NetworkId) return;

            switch (e.NewState)
            {
                case MollState.Emerging:
                    _animator.SetTrigger("Emerge");
                    break;
                case MollState.Hit:
                    _animator.SetTrigger("Hit");
                    // Play hit effect
                    break;
                case MollState.Hiding:
                    _animator.SetTrigger("Hide");
                    break;
            }
        }
    }
}