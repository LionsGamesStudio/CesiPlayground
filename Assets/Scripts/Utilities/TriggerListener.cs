using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Utilities
{
    /// <summary>
    /// To put on objects you want to add action on their trigger from other script
    /// </summary>
    public class TriggerListener : MonoBehaviour
    {
        public UnityAction<Collider, GameObject> onTriggerEnter;
        public UnityAction<Collider, GameObject> onTriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke(other, this.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit?.Invoke(other, this.gameObject);
        }
    }
}
