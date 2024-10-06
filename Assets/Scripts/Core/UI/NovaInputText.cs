using Assets.Scripts.Core.Events.UI;
using Assets.Scripts.Core.Events;
using Nova;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Nova.Gesture;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using Nova.TMP;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Core.UI
{
    [RequireComponent(typeof(CoreBlock), typeof(BoxCollider), typeof(Rigidbody))]
    public class NovaInputText : MonoBehaviour
    {
        [Header("Keyboard Settings")]
        public TextMeshProTextBlock TextBlock;

        public float Distance = 0.5f;
        public float VerticalOffset = -0.5f;

        public Transform PositionSource;

        [Header("Input Parameters")]
        [SerializeField] private bool _isPassword = false;
        [SerializeField] private bool _disableKeyboardInput = false;

        [SerializeField] private UIBlock _background;
        [SerializeField] private Color _classicColor;
        [SerializeField] private Color _hoverColor;

        private EventBinding<NovaUIHoverEvent> _onHover;
        private EventBinding<NovaUISelectEvent> _selectBinding;

        private UIBlock3D _button;
        private BoxCollider _collider;
        private Rigidbody _rigidbody;

        private string _placeholder = "";
        private string _text = "";

        /// <summary>
        /// Opens the keyboard for the input text
        /// </summary>
        public void OpenKeyboard()
        {
            if(this.enabled == false)
            {
                return;
            }

            //EventSystem.current.

            // Remove the previous listeners
            NonNativeKeyboard.Instance.InputField.onValueChanged.RemoveAllListeners();

            // Set the input field value to the text block value
            NonNativeKeyboard.Instance.InputField.text = TextBlock.text == _placeholder ? "" : _text;

            // Update the text block with the input field value
            NonNativeKeyboard.Instance.InputField.onValueChanged.AddListener(x =>
            {
                if(x.Length <= 0)
                {
                    TextBlock.text = _placeholder;
                }
                else
                {
                    TextBlock.text = "";
                    TextBlock.text = _isPassword ? new string('*', x.Length) : x;
                    _text = x;
                }
            });

            NonNativeKeyboard.Instance.PresentKeyboard();

            // Calculate the position of the keyboard
            Vector3 direction = PositionSource.forward;
            direction.y = 0;
            direction.Normalize();

            Vector3 targetPosition = PositionSource.position + direction * Distance + Vector3.up * VerticalOffset;

            NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);
        }


        #region Unity Methods

        private void Awake()
        {
            _button = GetComponent<UIBlock3D>();
            _collider = GetComponent<BoxCollider>();
            _rigidbody = GetComponent<Rigidbody>();

            if (_button == null)
            {
                Debug.LogError("NovaButton requires a UIBlock3D component to function.");
            }

            if (_collider == null)
            {
                Debug.LogError("NovaButton requires a BoxCollider component to function.");
            }

            if (_rigidbody == null)
            {
                Debug.LogError("NovaButton requires a Rigidbody component to function.");
            }

            _placeholder = TextBlock.text;
        }

        private void Update()
        {
            _collider.size = new Vector3(_button.Size.Value.x, _button.Size.Value.y, _button.Size.Value.z * 5);
        }

        private void OnEnable()
        {
            _selectBinding = new EventBinding<NovaUISelectEvent>(Select);
            _onHover = new EventBinding<NovaUIHoverEvent>(Hover);

            EventBus<NovaUISelectEvent>.Register(_selectBinding);
            EventBus<NovaUIHoverEvent>.Register(_onHover);
        }

        private void OnDisable()
        {
            EventBus<NovaUISelectEvent>.Unregister(_selectBinding);
            EventBus<NovaUIHoverEvent>.Unregister(_onHover);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the select event for the button
        /// </summary>
        /// <param name="e"></param>
        private void Select(NovaUISelectEvent e)
        {
            if (e.Element == _button)
            {
                OpenKeyboard();
            }
        }

        /// <summary>
        /// Handles the hover event for the button
        /// </summary>
        /// <param name="e"></param>
        private void Hover(NovaUIHoverEvent e)
        {
            if (e.Element == _button)
            {
                _background.Color = _hoverColor;
            }
            else
            {
                _background.Color = _classicColor;
            }
        }

        #endregion

        public string Text
        {
            get
            {
                return _text;
            }
        }

    }
}
