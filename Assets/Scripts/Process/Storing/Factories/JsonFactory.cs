using Assets.Scripts.Core.Storing;
using Assets.Scripts.Process.Storing.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Process.Storing.Factories
{
    [CreateAssetMenu(fileName = "new Json Factory", menuName = "Storage/JsonStorageFactory", order = 1)]
    public class JsonFactory : IStorageFactory
    {
        public override IStorage CreateStorage()
        {
            return new Json();
        }
    }
}
