using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Storing
{
    public abstract class IStorageFactory : ScriptableObject
    {
        /// <summary>
        /// Create a storage object
        /// </summary>
        /// <returns>The storage created</returns>
        public abstract IStorage CreateStorage();
    }

}