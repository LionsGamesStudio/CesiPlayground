using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Parameters
{
    public class Parameter : MonoBehaviour
    {
        public Camera XRCamera;

        public void Update()
        {
            transform.rotation = XRCamera.transform.rotation;
        }
    }
}
