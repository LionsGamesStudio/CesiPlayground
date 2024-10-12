using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.API;
using Assets.Scripts.Core.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.API
{
    public class APIManager : MonoBehaviour
    {
        [SerializeField] private StorageManager _storageManager;

        [SerializeField] private List<APIRequester> requests = new List<APIRequester>();

        private EventBinding<OnAPIRequestEvent> _onAPIRequestBinding;

        public void Awake()
        {
            _onAPIRequestBinding = new EventBinding<OnAPIRequestEvent>(OnAPIRequest);

            EventBus<OnAPIRequestEvent>.Register(_onAPIRequestBinding);
        }

        /// <summary>
        /// Handle API requests
        /// </summary>
        /// <param name="e"></param>
        private void OnAPIRequest(OnAPIRequestEvent e)
        {
            APIRequester requester = requests.Find(r => r.GetType().Name == e.RequesterName);
            requester.SetStorageManager(_storageManager);
            if (requester != null)
            {
                var method = requester.GetType().GetMethod(e.Method);
                if (method != null)
                {
                    method?.Invoke(requester, new object[] { e.Data });
                }
                else
                {
                    Debug.LogError("Method " + e.Method + " not found in " + e.RequesterName);
                }
            }
            else
            {
                Debug.LogError("Requester " + e.RequesterName + " not found");
            }
        }
    }
}
