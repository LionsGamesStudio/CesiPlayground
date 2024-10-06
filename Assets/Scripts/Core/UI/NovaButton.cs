using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.UI;
using Nova;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Assets.Scripts.Core.UI
{
    [RequireComponent(typeof(CoreBlock), typeof(BoxCollider), typeof(Rigidbody))]
    public class NovaButton : MonoBehaviour
    {
        [Header("Button Events")]
        [SerializeField] private NovaEvent _onHoverEnter;
        [SerializeField] private NovaEvent _onHover;
        [SerializeField] private NovaEvent _onHoverExit;
        [SerializeField] private NovaEvent _onSelect;

        [Header("Button Settings")]
        [SerializeField] private UIBlock _background;
        [SerializeField] private Color _classicColor;
        [SerializeField] private Color _hoverColor;

        private EventBinding<NovaUIHoverEvent> _hoverEnterBinding;
        private EventBinding<NovaUIHoverEvent> _hoverExitBinding;
        private EventBinding<NovaUISelectEvent> _selectBinding;

        private UIBlock3D _button;
        private BoxCollider _collider;
        private Rigidbody _rigidbody;
        private bool _newHover = true;

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
        }

        private void Update()
        {
            _collider.size = new Vector3(_button.Size.Value.x, _button.Size.Value.y, _button.Size.Value.z * 5);
        }

        private void OnEnable()
        {
            _hoverEnterBinding = new EventBinding<NovaUIHoverEvent>(Hover);
            _hoverExitBinding = new EventBinding<NovaUIHoverEvent>(HoverExit);
            _selectBinding = new EventBinding<NovaUISelectEvent>(Select);

            EventBus<NovaUIHoverEvent>.Register(_hoverEnterBinding);
            EventBus<NovaUIHoverEvent>.Register(_hoverExitBinding);
            EventBus<NovaUISelectEvent>.Register(_selectBinding);
        }

        private void OnDisable()
        {
            EventBus<NovaUIHoverEvent>.Unregister(_hoverEnterBinding);
            EventBus<NovaUIHoverEvent>.Unregister(_hoverExitBinding);
            EventBus<NovaUISelectEvent>.Unregister(_selectBinding);
        }

        #region Getters & Setters

        public NovaEvent OnHoverEnter { get => _onHoverEnter; set => _onHoverEnter = value; }
        public NovaEvent OnHover { get => _onHover; set => _onHover = value; }
        public NovaEvent OnHoverExit { get => _onHoverExit; set => _onHoverExit = value; }
        public NovaEvent OnSelect { get => _onSelect; set => _onSelect = value; }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the hover event for the button
        /// </summary>
        /// <param name="e"></param>
        private void Hover(NovaUIHoverEvent e)
        {
            // Check if the button is being hovered over
            if (e.Element == _button && e.IsHovering)
            {
                if (_newHover)
                {
                    _onHoverEnter?.Invoke();
                    _newHover = false;
                }
                else
                {
                    _onHover?.Invoke();
                    _background.Color = _hoverColor;
                }
            }
        }

        /// <summary>
        /// Handles the hover exit event for the button
        /// </summary>
        /// <param name="e"></param>
        private void HoverExit(NovaUIHoverEvent e)
        {
            if (e.Element == _button && !e.IsHovering && !_newHover)
            {
                _onHoverExit?.Invoke();
                _newHover = true;
                _background.Color = _classicColor;
            }
        }

        /// <summary>
        /// Handles the select event for the button
        /// </summary>
        /// <param name="e"></param>
        private void Select(NovaUISelectEvent e)
        {
            if (e.Element == _button)
            {
                _onSelect?.Invoke();
            }
        }

        #endregion
    }
}
