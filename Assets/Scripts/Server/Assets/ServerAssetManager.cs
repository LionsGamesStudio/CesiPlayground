using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Shared.Data.Games;

namespace CesiPlayground.Server.Assets
{
    /// <summary>
    /// A server-side service that automatically discovers and pre-loads all ScriptableObject
    /// assets from any "Resources" folder at startup. This provides fast, synchronous
    /// access to all game data for the server logic.
    /// </summary>
    public class ServerAssetManager
    {
        private readonly Dictionary<string, ScriptableObject> _loadedAssets = new Dictionary<string, ScriptableObject>();

        public ServerAssetManager()
        {
            ServiceLocator.Register(this);
            LoadAllGameData();
        }

        /// <summary>
        /// Scans all "Resources" folders, loads all ScriptableObjects, and
        /// registers them in a dictionary for fast retrieval.
        /// </summary>
        private void LoadAllGameData()
        {
            // Resources.LoadAll with an empty string scans all Resources folders in the project.
            var allAssets = Resources.LoadAll<ScriptableObject>("");

            foreach (var asset in allAssets)
            {
                // We have a special rule for DataGame assets: they are keyed by their internal 'GameId' property.
                if (asset is DataGame dataGame)
                {
                    if (!string.IsNullOrEmpty(dataGame.GameId))
                    {
                        if (_loadedAssets.ContainsKey(dataGame.GameId))
                        {
                            Debug.LogWarning($"[ServerAssetManager] Duplicate GameId '{dataGame.GameId}' found. Asset '{asset.name}' will be ignored.");
                            continue;
                        }
                        _loadedAssets[dataGame.GameId] = dataGame;
                        Debug.Log($"[ServerAssetManager] Loaded DataGame '{asset.name}' with key '{dataGame.GameId}'");
                    }
                    else
                    {
                        Debug.LogWarning($"[ServerAssetManager] DataGame asset '{asset.name}' has a null or empty GameId. It will be keyed by its filename instead.");
                        _loadedAssets[asset.name] = asset;
                    }
                }
                // For all other ScriptableObjects, we use their filename as the key.
                else
                {
                    if (_loadedAssets.ContainsKey(asset.name))
                    {
                        Debug.LogWarning($"[ServerAssetManager] Duplicate asset name '{asset.name}' found. The new asset will overwrite the old one in the registry.");
                    }
                    _loadedAssets[asset.name] = asset;
                }
            }
            
            Debug.Log($"[ServerAssetManager] Finished loading. A total of {_loadedAssets.Count} assets are registered.");
        }

        /// <summary>
        /// Synchronously gets a pre-loaded data asset.
        /// </summary>
        /// <param name="key">The key of the asset (either the GameId for a DataGame, or the filename for others).</param>
        /// <returns>The loaded asset, or null if not found.</returns>
        public T GetAsset<T>(string key) where T : ScriptableObject
        {
            if (_loadedAssets.TryGetValue(key, out var asset))
            {
                if (asset is T typedAsset)
                {
                    return typedAsset;
                }
                
                Debug.LogError($"[ServerAssetManager] Asset with key '{key}' was found, but it is of type '{asset.GetType().Name}', not the requested type '{typeof(T).Name}'.");
                return null;
            }
            
            Debug.LogError($"[ServerAssetManager] Asset with key '{key}' not found or not loaded.");
            return null;
        }
    }
}