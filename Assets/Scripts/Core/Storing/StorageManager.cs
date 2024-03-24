using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Storing
{
    public class StorageManager : MonoBehaviour
    {
        public IStorageFactory StorageFactory;

        private IStorage storage;

        public void Start()
        {
            if (StorageFactory == null) return;

            storage = StorageFactory.CreateStorage();
        }

        public T Get<T>(string filepath) where T : class
        {
            return storage.Get<T>(filepath);
        }

        public T GetFromString<T>(string text) where T : class
        {
            return storage.GetFromString<T>(text);
        }

        public void Store<T>(string filepath, T value) where T : class
        {
            storage.Store<T>(filepath, value);
        }
    }
}