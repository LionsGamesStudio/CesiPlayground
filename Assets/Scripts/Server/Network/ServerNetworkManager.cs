using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CesiPlayground.Shared.Network;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;
using CesiPlayground.Shared.Events.World;
using CesiPlayground.Server.API;
using CesiPlayground.Server.Players;
using CesiPlayground.Shared.Data;
using CesiPlayground.Shared.API;
using CesiPlayground.Shared.Data.Players;
using CesiPlayground.Server.Games;

namespace CesiPlayground.Server.Network
{
    /// <summary>
    /// Handles a single client connection on the server. Its only role is to
    /// receive messages and notify the manager of its lifecycle events.
    /// </summary>
    public class ClientConnection : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Debug.Log($"[Server] New anonymous client connected! Connection ID: {ID}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            ServiceLocator.Get<ServerNetworkManager>().QueueMessage(ID, e.Data);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Debug.Log($"[Server] Client connection closed. ID: {ID}, Reason: {e.Reason}");
            ServiceLocator.Get<ServerNetworkManager>().OnClientDisconnected(ID);
        }
    }
    
    /// <summary>
    /// The main network manager for the server. It starts the WebSocket server,
    /// handles client lifecycles, authenticates players, and translates
    /// network messages to server-side events.
    /// </summary>
    public class ServerNetworkManager : MonoBehaviour
    {
        [SerializeField] private int port = 9001;
        [SerializeField] private ServerAPIService apiService;

        private WebSocketServer _wssv;
        private readonly ConcurrentQueue<(string clientId, string data)> _messageQueue = new ConcurrentQueue<(string, string)>();
        private readonly Dictionary<string, string> _connectionIdToPlayerId = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _playerIdToConnectionId = new Dictionary<string, string>();
        private ServerPlayerManager _playerManager;
        private MatchmakingService _matchmakingService;
        private float _lastUpdateTime;

        private void Awake()
        {
            ServiceLocator.Register(this);
            if (apiService == null) { Debug.LogError("ServerAPIService is not assigned!"); }

            _playerManager = ServiceLocator.Get<ServerPlayerManager>();
            _wssv = new WebSocketServer($"ws://0.0.0.0:{port}");
            _wssv.AddWebSocketService<ClientConnection>("/Game");
            _wssv.Start();
            Debug.Log($"[Server] WebSocket Server started on port {port}...");
        }

        private void Start()
        {
            _matchmakingService = ServiceLocator.Get<MatchmakingService>();
        }

        private void Update()
        {
            while (_messageQueue.TryDequeue(out var message)) { ProcessMessage(message.clientId, message.data); }
            _lastUpdateTime = Time.time;
        }

        private void OnDestroy()
        {
            _wssv?.Stop();
            ServiceLocator.Unregister<ServerNetworkManager>();
        }

        public void QueueMessage(string clientId, string data) => _messageQueue.Enqueue((clientId, data));
        
        public void OnClientDisconnected(string connectionId)
        {
            if (_connectionIdToPlayerId.TryGetValue(connectionId, out string playerId))
            {
                _playerManager?.RemovePlayer(playerId);
                _playerIdToConnectionId.Remove(playerId);
                _connectionIdToPlayerId.Remove(connectionId);
            }
        }

        private async void ProcessMessage(string clientId, string jsonData)
        {
            var message = JsonSerialization.FromJson<INetworkMessage>(jsonData);

            if (message is LoginRequestMessage loginMsg)
            {
                PlayerData playerData = await apiService.VerifyCredentialsAsync(loginMsg.Username, loginMsg.Password);
                var response = new LoginResponseMessage();
                if (playerData != null)
                {
                    response.Success = true;
                    response.PlayerData = playerData;
                    _connectionIdToPlayerId[clientId] = playerData.PlayerId;
                    _playerIdToConnectionId[playerData.PlayerId] = clientId;
                    _playerManager.AddPlayer(new ServerPlayerEntity(playerData.PlayerId, Vector3.zero));
                }
                else { response.Success = false; response.ErrorMessage = "Invalid credentials."; }
                SendToClient(clientId, response);
                return;
            }

            if (message is RegisterRequestMessage registerMsg)
            {
                Debug.Log($"[Server] Received register request for user '{registerMsg.Username}' from connection {clientId}.");
                
                string apiResponse = await apiService.CreateAccountAsync(registerMsg.Username, registerMsg.Password);
                
                var response = new RegisterResponseMessage();
                if (apiResponse != null)
                {
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.ErrorMessage = "Registration failed. Username may be taken.";
                }
                
                SendToClient(clientId, response);
                return;
            }
            
            if (!_connectionIdToPlayerId.TryGetValue(clientId, out string playerId)) return;

            switch (message)
            {
                case QueueForGameMessage queueMsg:
                    _matchmakingService.HandleQueueRequest(playerId, queueMsg.GameId);
                    break;
                case PlayerActionMessage actionMsg:
                    GameEventSystem.Server.Raise(new ServerPlayerActionEvent { PlayerId = playerId, ActionType = actionMsg.actionType, State = actionMsg.state });
                    break;
                case PlayerUsedGateMessage gateMsg:
                    GameEventSystem.Server.Raise(new ServerPlayerUsedGateEvent { PlayerId = playerId, GateName = gateMsg.gateName });
                    break;
                case ClientReadyInSceneMessage readyMsg:
                    if (_playerManager.GetPlayer(playerId) is ServerPlayerEntity playerEntityReady)
                    {
                        playerEntityReady.SetPositionWithoutCorrection(readyMsg.Position, readyMsg.Rotation);
                        Debug.Log($"[Server] Player {playerId} is now ready in new scene at {readyMsg.Position}");
                    }
                    break;
                case PlayerTransformUpdateMessage transformMsg:
                    if (_playerManager.GetPlayer(playerId) is ServerPlayerEntity playerEntityToTransform)
                        playerEntityToTransform.UpdateTransform(transformMsg.Position, transformMsg.Rotation, Time.time - _lastUpdateTime);
                    break;
                default:
                    Debug.LogWarning($"[Server] Unhandled message: {message.GetType()} from {playerId}");
                    break;
            }
        }

        public void Broadcast(INetworkMessage message)
        {
            string jsonData = JsonSerialization.ToJson(message);
            _wssv.WebSocketServices["/Game"].Sessions.Broadcast(jsonData);
        }
        
        public void SendToClient(string connectionId, INetworkMessage message)
        {
            string jsonData = JsonSerialization.ToJson(message);
            if (_wssv.WebSocketServices["/Game"].Sessions.TryGetSession(connectionId, out var session))
                session.Context.WebSocket.Send(jsonData);
        }

        public void SendToPlayer(string playerId, INetworkMessage message)
        {
            if (_playerIdToConnectionId.TryGetValue(playerId, out string connectionId))
                SendToClient(connectionId, message);
        }
    }
}