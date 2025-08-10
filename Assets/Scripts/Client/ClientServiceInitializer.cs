using UnityEngine;
using CesiPlayground.Client.API;
using CesiPlayground.Client.Players;
using CesiPlayground.Client.Addressables;
using CesiPlayground.Core;
using CesiPlayground.Client.UI;

namespace CesiPlayground.Client
{
    /// <summary>
    /// A client-side bootstrapper that creates and registers all necessary
    /// non-MonoBehaviour services at the start of the application.
    /// </summary>
    public class ClientServiceInitializer : MonoBehaviour
    {
        [Tooltip("Assign the ScriptableObject asset for the API service here.")]
        [SerializeField] private ClientAPIService clientApiService;

        [Header("Service Prefabs")]
        [Tooltip("Assign your Fader_Prefab here.")]
        [SerializeField] private GameObject faderPrefab;

        private void Awake()
        {
            // --- 1. Instantiate Service Prefabs ---
            if (faderPrefab != null)
            {
                // We only instantiate it if an instance doesn't already exist from a previous scene load.
                if (!Fader.HasInstance)
                {
                    Instantiate(faderPrefab);
                }
            }
            else
            {
                Debug.LogError("Fader Prefab is not assigned in the ClientServiceInitializer!");
            }

            // --- 2. Register ScriptableObject Services ---
            if (clientApiService != null)
            {
                ServiceLocator.Register(clientApiService);
            }
            else
            {
                Debug.LogError("ClientAPIService asset is not assigned in the ClientServiceInitializer!");
            }

            // --- 3. Create and Register Plain C# Services ---
            new AddressablesManager();
            new PlayerDataService();
        }
    }
}