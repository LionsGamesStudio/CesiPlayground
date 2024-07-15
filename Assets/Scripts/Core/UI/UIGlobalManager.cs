using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.UI;
using Assets.Scripts.Core.Events.XR;
using Assets.Scripts.Core.Games;
using Assets.Scripts.Core.Storing;
using Assets.Scripts.Core.XR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.UI
{
    public class UIGlobalManager : MonoBehaviour
    {
        private List<Blackboard> _uiElemList = new List<Blackboard>();

        private EventBinding<UIRegisterElement> _registerElementBinding;
        private EventBinding<UIUnregisterElement> _unregisterElementBinding;

        public void Awake()
        {
            _registerElementBinding = new EventBinding<UIRegisterElement>(OnRegisterElement);
            _unregisterElementBinding = new EventBinding<UIUnregisterElement>(OnUnregisterElement);

            EventBus<UIRegisterElement>.Register(_registerElementBinding);
            EventBus<UIUnregisterElement>.Register(_unregisterElementBinding);

        }

        #region Event Handlers

        /// <summary>
        /// Handle the event when an element is registered
        /// </summary>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        public void OnRegisterElement(UIRegisterElement e)
        {
            if (!_uiElemList.Any(x => x.Get<int>("id") == e.Id))
            {
                Blackboard bb = new Blackboard();
                bb.Set("id", e.Id);
                bb.Set("element", e.Element);
            }
            else
                throw new Exception("Element already registered");
        }

        /// <summary>
        /// Handle the event when an element is unregistered
        /// </summary>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        public void OnUnregisterElement(UIUnregisterElement e)
        {
            if (_uiElemList.Any(x => x.Get<int>("id") == e.Id))
            {
                var elem = _uiElemList.First(x => x.Get<int>("id") == e.Id);
                _uiElemList.Remove(elem);
            }
            else
                throw new Exception("Element not registered");
        }

        #endregion
    }
}
