using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Server.API;
using CesiPlayground.Server.Assets;
using CesiPlayground.Server.Games;
using CesiPlayground.Server.Network;
using CesiPlayground.Server.Players;
using CesiPlayground.Server.Scoring;
using CesiPlayground.Server.Spawning;
using CesiPlayground.Server.AI;
using CesiPlayground.Server.Gates;
using CesiPlayground.Shared.Data.Games;
using CesiPlayground.Shared.Data.HubLayoutData;

namespace CesiPlayground.Server
{
    public class ServerGameLoop : MonoBehaviour
    {
        [Header("Required Scene Assets")]
        [Tooltip("Assign the ServerAPIService ScriptableObject asset here.")]
        [SerializeField] private ServerAPIService serverApiService;

        [Tooltip("Assign the GameRegistry ScriptableObject asset here.")]
        [SerializeField] private GameRegistry gameRegistry;

        private ServerGameManager _serverGameManager;
        private ServerEntityManager _serverEntityManager;
        private ServerAssetManager _serverAssetManager;

        private void Awake()
        {
            // --- 1. Register ScriptableObject services first ---
            if (serverApiService != null) ServiceLocator.Register(serverApiService);
            else Debug.LogError("ServerAPIService is not assigned in the inspector!");

            if (gameRegistry == null)
            {
                Debug.LogError("GameRegistry is not assigned in the inspector!");
                return;
            }

            // --- 2. Create and register plain C# services ---
            _serverAssetManager = new ServerAssetManager();
            new ServerObjectManager();
            new ScoreService();
            new ServerPlayerManager();
            new ServerGateService();
            _serverEntityManager = new ServerEntityManager();
            _serverGameManager = new ServerGameManager(gameRegistry);
            new MatchmakingService();

            // --- 3. Ensure MonoBehaviour services exist ---
            if (GetComponent<ServerNetworkManager>() == null)
            {
                Debug.LogError("ServerNetworkManager component is missing from the [ServerLoop] GameObject!");
            }
        }

        private void Start()
        {
            InitializeGamesFromLayout();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            _serverGameManager?.Update(deltaTime);
            _serverEntityManager?.Update(deltaTime);
        }
        
        private void InitializeGamesFromLayout()
        {
            var assetManager = ServiceLocator.Get<Assets.ServerAssetManager>();
            var hubLayout = assetManager.GetAsset<HubLayoutData>("HubLayoutData");

            if (hubLayout == null)
            {
                Debug.LogError("HubLayoutData.asset not found! Make sure to bake the Hub scene.");
                return;
            }
            
            Debug.Log($"[Server] Initializing {hubLayout.StandConfigurations.Count} games from Hub layout...");

            foreach (var standConfig in hubLayout.StandConfigurations)
            {
                var gameData = assetManager.GetAsset<DataGame>(standConfig.GameDataId);
                if (gameData != null)
                {
                    _serverGameManager.CreateGameSession(gameData, standConfig.StandPosition, standConfig.SpawnPoints);
                }
                else
                {
                    Debug.LogWarning($"Could not find DataGame with ID '{standConfig.GameDataId}'");
                }
            }
        }
    }
}