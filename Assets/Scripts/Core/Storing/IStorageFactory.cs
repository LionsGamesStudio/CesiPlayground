using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.Storing
{
    public abstract class IStorageFactory : ScriptableObject
    {
        public abstract IStorage CreateStorage();
    }

}