using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;
using CesiPlayground.Shared.Events.World;
using CesiPlayground.Shared.Network;
using CesiPlayground.Shared.Events.Client;

namespace CesiPlayground.Client.Network
{
    public class ClientNetworkInput : MonoBehaviour
    {
        private ClientNetworkManager _networkManager;

        private void Start()
        {
            _networkManager = ServiceLocator.Get<ClientNetworkManager>();
        }

        private void OnEnable()
        {
            GameEventSystem.Client.Register<ClientPlayerActionEvent>(OnPlayerAction);
            GameEventSystem.Client.Register<PlayerMovementInputEvent>(OnPlayerMovement);
            GameEventSystem.Client.Register<ClientPlayerUsedGateEvent>(OnPlayerUsedGate);
            GameEventSystem.Client.Register<PlayerQueueForGameEvent>(OnQueueForGame);
        }

        private void OnDisable()
        {
            GameEventSystem.Client.Unregister<ClientPlayerActionEvent>(OnPlayerAction);
            GameEventSystem.Client.Unregister<PlayerMovementInputEvent>(OnPlayerMovement);
            GameEventSystem.Client.Unregister<ClientPlayerUsedGateEvent>(OnPlayerUsedGate);
            GameEventSystem.Client.Unregister<PlayerQueueForGameEvent>(OnQueueForGame);
        }

        /// <summary>
        /// Translates the local player action input into a network message.
        /// This is typically called when the player performs an action.
        /// </summary>
        private void OnPlayerAction(ClientPlayerActionEvent e)
        {
            if (_networkManager == null) return;
            var message = new PlayerActionMessage { actionType = e.ActionType, state = e.State };
            _networkManager.Send(message);
        }

        /// <summary>
        /// Translates the local player movement input into a network message.
        /// This is typically called when the player moves.
        /// </summary>
        private void OnPlayerMovement(PlayerMovementInputEvent e)
        {
            if (_networkManager == null) return;
            var message = new PlayerMovementMessage { moveValue = e.MoveValue };
            _networkManager.Send(message);
        }

        /// <summary>
        /// Translates the local "used gate" event into a network message.
        /// </summary>
        private void OnPlayerUsedGate(ClientPlayerUsedGateEvent e)
        {
            if (_networkManager == null) return;
            var message = new PlayerUsedGateMessage { gateName = e.GateName };
            _networkManager.Send(message);
        }

        /// <summary>
        /// Translates the local "queue for game" event into a network message.
        /// </summary>
        private void OnQueueForGame(PlayerQueueForGameEvent e)
        {
            if (_networkManager == null) return;
            var message = new QueueForGameMessage { GameId = e.GameId };
            _networkManager.Send(message);
        }
    }
}