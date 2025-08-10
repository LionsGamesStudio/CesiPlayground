using UnityEngine;
using CesiPlayground.Core.Storing;

namespace CesiPlayground.Client.Storing.Factories
{
    /// <summary>
    /// A factory for creating instances of the JsonStorage implementation.
    /// </summary>
    [CreateAssetMenu(fileName = "JsonStorageFactory", menuName = "Storage/JsonStorageFactory", order = 1)]
    public class JsonStorageFactory : IStorageFactory
    {
        public override IStorage CreateStorage()
        {
            return new JsonStorage();
        }
    }
}