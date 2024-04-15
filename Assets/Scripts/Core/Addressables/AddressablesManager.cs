using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Addressable = UnityEngine.AddressableAssets.Addressables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Assets.Scripts.Core.Addressables
{
    public class AddressablesManager
    {
        private static AddressablesManager instance;

        private AsyncOperationHandle loadHandle;

        private AddressablesManager() { }
        public static AddressablesManager Instance
        {
            get
            {
                if(instance == null)
                    instance = new AddressablesManager();
                return instance;
            }
        }


        #region Load
        /// <summary>
        /// Load an asset based on his addressable address
        /// </summary>
        /// <typeparam name="T">The type of the asset</typeparam>
        /// <param name="address">The address of the asset</param>
        /// <returns></returns>
        public T LoadAddressableAsync<T>(string address)
        {
            loadHandle = Addressable.LoadAssetAsync<T>(address);

            loadHandle.WaitForCompletion();

            return (T)loadHandle.Result;
        }

        /// <summary>
        /// Load assets with the same addressable label
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <returns></returns>
        public List<T> LoadAddressableFromLabel<T>(string label)
        {
            AsyncOperationHandle<IList<IResourceLocation>> handle;

            handle = Addressable.LoadResourceLocationsAsync(label);

            handle.WaitForCompletion();

            List<T> result = new List<T>();

            foreach(IResourceLocation resource in handle.Result)
            {
                result.Add(LoadAddressableAsync<T>(resource.PrimaryKey));
            }

            Addressable.Release(handle);

            return result;
        }

        #endregion
    }
}