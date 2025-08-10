using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using CesiPlayground.Core;

namespace CesiPlayground.Client.Addressables
{
    /// <summary>
    /// A client-side service for loading game assets via the Addressables system.
    /// This should not be used by the server.
    /// </summary>
    public class AddressablesManager
    {        
        /// <summary>
        /// Registers this service with the ServiceLocator upon creation.
        /// </summary>
        public AddressablesManager()
        {
            ServiceLocator.Register(this);
        }
        
        /// <summary>
        /// Loads an asset based on its addressable address.
        /// Note: This is a synchronous wrapper. For smoother loading, use the async version.
        /// </summary>
        /// <typeparam name="T">The type of the asset.</typeparam>
        /// <param name="address">The address of the asset.</param>
        /// <returns>The loaded asset.</returns>
        public T LoadAsset<T>(string address) where T : class
        {
            AsyncOperationHandle<T> loadHandle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(address);
            loadHandle.WaitForCompletion();
            
            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                return loadHandle.Result;
            }
            Debug.LogError($"Failed to load addressable asset: {address}");
            return null;
        }

        /// <summary>
        /// Loads all assets with the same addressable label.
        /// </summary>
        /// <typeparam name="T">The type of assets to load.</typeparam>
        /// <param name="label">The label to search for.</param>
        /// <returns>A list of loaded assets.</returns>
        public List<T> LoadAssetsByLabel<T>(string label) where T : class
        {
            AsyncOperationHandle<IList<T>> handle = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(label, null);
            handle.WaitForCompletion();
            
            List<T> result = new List<T>();
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                result.AddRange(handle.Result);
            }
            else
            {
                Debug.LogError($"Failed to load addressable assets with label: {label}");
            }
            
            UnityEngine.AddressableAssets.Addressables.Release(handle);
            return result;
        }
    }
}