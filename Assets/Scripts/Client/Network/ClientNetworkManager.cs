using UnityEngine;
using WebSocketSharp;
using System.Collections.Concurrent;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Network;
using CesiPlayground.Shared.API;
using CesiPlayground.Shared.Events.Spawn;
using CesiPlayground.Shared.Events.Game;
using CesiPlayground.Shared.Events.Data;
using CesiPlayground.Shared.Events.Client;


namespace CesiPlayground.Client.Network
{
    public class ClientNetworkManager : MonoBehaviour
    {
        private WebSocket _ws;
        private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

        private void Awake()
        {
            ServiceLocator.Register(this);
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Connect();
        }

        private void Update()
        {
            while (_messageQueue.TryDequeue(out var data))
            {
                ProcessMessage(data);
            }
        }

        private void OnDestroy()
        {
            _ws?.Close();
            ServiceLocator.Unregister<ClientNetworkManager>();
        }

        public void Connect(string address = "ws://127.0.0.1:9001/Game")
        {
            if (_ws != null && (_ws.IsAlive || _ws.ReadyState == WebSocketState.Connecting)) return;

            _ws = new WebSocket(address);
            _ws.OnOpen += (sender, e) => Debug.Log("[Client] Connected to server!");
            _ws.OnMessage += (sender, e) => _messageQueue.Enqueue(e.Data);
            _ws.OnError += (sender, e) => Debug.LogError($"[Client] WebSocket Error: {e.Message}");
            _ws.OnClose += (sender, e) => Debug.Log("[Client] Disconnected from server.");
            _ws.Connect();
        }

        public void Send(INetworkMessage message)
        {
            if (_ws == null || !_ws.IsAlive)
            {
                Debug.LogError("[Client] Not connected to server. Cannot send message.");
                return;
            }

            string jsonData = JsonSerialization.ToJson(message);
            _ws.Send(jsonData);
        }

        private void ProcessMessage(string jsonData)
        {
            var message = JsonSerialization.FromJson<INetworkMessage>(jsonData);

            switch (message)
            {
                case LoginResponseMessage loginResponse:
                    if (loginResponse.Success)
                    {
                        GameEventSystem.Client.Raise(new PlayerDataLoadedEvent { Data = loginResponse.PlayerData });
                    }
                    else
                    {
                        Debug.LogError($"[Client] Login failed: {loginResponse.ErrorMessage}");
                    }
                    break;
                case RegisterResponseMessage registerResponse:
                    if (registerResponse.Success)
                    {
                        Debug.Log("[Client] Registration successful! Please log in.");
                        // TODO: Raise a "RegisterSuccess" event for the UI to maybe
                        // automatically switch to the login tab.
                    }
                    else
                    {
                        Debug.LogError($"[Client] Registration failed: {registerResponse.ErrorMessage}");
                    }
                    break;

                case PlayerTransformCorrectionMessage correctionMsg:
                    GameEventSystem.Client.Raise(new PlayerTransformCorrectionEvent { Position = correctionMsg.Position, Rotation = correctionMsg.Rotation });
                    break;

                case SpawnObjectMessage spawnMsg:
                    GameEventSystem.Client.Raise(new SpawnNetworkedObjectEvent { NetworkId = spawnMsg.networkId, PrefabId = spawnMsg.prefabId, Position = spawnMsg.position, Rotation = spawnMsg.rotation, InitialVelocity = spawnMsg.initialVelocity });
                    break;
                case DestroyObjectMessage destroyMsg:
                    GameEventSystem.Client.Raise(new DestroyNetworkedObjectEvent { NetworkId = destroyMsg.networkId });
                    break;

                case InstantiateGameMessage instantiateMsg:
                    GameEventSystem.Client.Raise(new InstantiateGameEvent 
                    {
                        InstanceId = instantiateMsg.gameInstanceId,
                        GameId = instantiateMsg.gameDataId,
                        Position = instantiateMsg.position
                    });
                    break;
                
                case ReturnToHubMessage returnMsg:
                    GameEventSystem.Client.Raise(new ReturnToHubEvent
                    {
                        InstanceId = returnMsg.gameInstanceId,
                        HubReturnPosition = returnMsg.hubReturnPosition
                    });
                    break;

                case GameStateUpdateMessage stateMsg:
                    GameEventSystem.Client.Raise(new GameStateUpdatedEvent { GameId = stateMsg.gameInstanceId, LocalPlayerScore = stateMsg.localPlayerScore });
                    break;
                case GameStatusMessage statusMsg:
                    GameEventSystem.Client.Raise(new GameStatusUpdatedEvent { GameInstanceId = statusMsg.gameInstanceId, StatusText = statusMsg.statusText });
                    break;
                case GameLivesUpdatedMessage livesMsg:
                    GameEventSystem.Client.Raise(new GameLivesUpdatedEvent { GameInstanceId = livesMsg.gameInstanceId, RemainingLives = livesMsg.remainingLives });
                    break;

                default:
                    Debug.LogWarning($"[Client] Received unhandled message type: {message.GetType()}");
                    break;
            }
        }
    }
}