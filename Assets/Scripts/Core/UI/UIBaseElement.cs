using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.UI;
using Assets.Scripts.Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Core.UI
{
    public abstract class UIBaseElement : MonoBehaviour
    {
        protected int _id;
        protected bool _registered = false;

        #region Basic Functions

        public virtual void Awake()
        {
            _id = gameObject.GetInstanceID();
        }

        public virtual void Start()
        {
            // Register this element
            Register();
        }

        public virtual void OnDestroy()
        {
            // Unregister this element
            Unregister();
        }

        public virtual void OnEnable()
        {
            // Register this element
            Register();
        }

        public virtual void OnDisable()
        {
            // Unregister this element
            Unregister();
        }

        #endregion

        /// <summary>
        /// Show the element
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);

            Register();
        }

        /// <summary>
        /// Hide the element
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);

            Unregister();
        }

        /// <summary>
        /// Toggle the visibility of the element
        /// </summary>
        public virtual void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);

            if (gameObject.activeSelf)
            {
                Register();
            }
            else
            {
                Unregister();
            }
        }

        #region Event Raise

        private void Register()
        {
            if(_registered)
                return;

            UIRegisterElement e = new UIRegisterElement() { Id = _id };

            EventBus<UIRegisterElement>.Raise(e);

            _registered = true;
        }

        private void Unregister()
        {
            if(!_registered)
                return;

            UIUnregisterElement e = new UIUnregisterElement() { Id = _id };

            EventBus<UIUnregisterElement>.Raise(e);

            _registered = false;
        }

        #endregion

    }
}
