using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Storing
{
    public class StorageManager : MonoBehaviour
    {
        [Tooltip("The factory to create the storage")]
        public IStorageFactory StorageFactory;

        /// <summary>
        /// Storage to used
        /// </summary>
        private IStorage storage;

        public void Start()
        {
            if (StorageFactory == null) return;

            storage = StorageFactory.CreateStorage();
        }

        #region Storage Functions

        /// <summary>
        /// Get a content from a file
        /// </summary>
        /// <typeparam name="T">Type of the content to get</typeparam>
        /// <param name="filepath">Filepath where content is stored</param>
        /// <returns>The content stored</returns>
        public T Get<T>(string filepath) where T : class
        {
            return storage.Get<T>(filepath);
        }

        /// <summary>
        /// Get a content from a string
        /// </summary>
        /// <typeparam name="T">Type of the content to get</typeparam>
        /// <param name="text">String containing the content</param>
        /// <returns>The content</returns>
        public T GetFromString<T>(string text) where T : class
        {
            return storage.GetFromString<T>(text);
        }

        /// <summary>
        /// Store a content in a file
        /// </summary>
        /// <typeparam name="T">Type of the content to store</typeparam>
        /// <param name="filepath">Filepath where storing the content</param>
        /// <param name="value">The content to stock</param>
        public void Store<T>(string filepath, T value) where T : class
        {
            storage.Store<T>(filepath, value);
        }

        #endregion
    }
}