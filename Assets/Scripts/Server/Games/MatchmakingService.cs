using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Server.Network;
using CesiPlayground.Shared.Network;
using CesiPlayground.Server.Assets;
using CesiPlayground.Shared.Data.Games;

namespace CesiPlayground.Server.Games
{
    public class MatchmakingService
    {
        private readonly ServerGameManager _gameManager;
        private readonly ServerNetworkManager _networkManager;
        private readonly ServerAssetManager _assetManager;
        private Vector3 _nextInstancePosition = new Vector3(0, 1000, 0);
        private Vector3 _instancePositionIncrement = new Vector3(200, 0, 0);

        private ServerGameManager GameManager => _gameManager != null ? _gameManager : ServiceLocator.Get<ServerGameManager>();
        private ServerNetworkManager NetworkManager => _networkManager != null ? _networkManager : ServiceLocator.Get<ServerNetworkManager>();
        private ServerAssetManager AssetManager => _assetManager != null ? _assetManager : ServiceLocator.Get<ServerAssetManager>();

        public MatchmakingService()
        {
            ServiceLocator.Register(this);
        }

        public void HandleQueueRequest(string playerId, string gameId)
        {
            var gameData = AssetManager.GetAsset<DataGame>(gameId);
            if (gameData == null) return;
            
            var session = GameManager.CreateGameSession(gameData, _nextInstancePosition);
            if (session == null) return;

            session.AddPlayer(playerId);
            
            NetworkManager.SendToPlayer(playerId, new InstantiateGameMessage
            {
                gameInstanceId = session.GameInstanceId,
                gameDataId = gameData.GameId,
                position = _nextInstancePosition
            });

            _nextInstancePosition += _instancePositionIncrement;
            session.StartGame();
        }
    }
}