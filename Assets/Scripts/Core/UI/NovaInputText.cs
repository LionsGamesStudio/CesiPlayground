using UnityEngine;
using Nova;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using CesiPlayground.Core.Events;
using CesiPlayground.Shared.Events.Input;
using Nova.TMP;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace CesiPlayground.Client.UI
{
    [RequireComponent(typeof(CoreBlock), typeof(XRSimpleInteractable))]
    public class NovaInputText : MonoBehaviour
    {
        [Header("Keyboard Settings")]
        public TextMeshProTextBlock TextBlock;
        public float Distance = 0.5f;
        public float VerticalOffset = -0.5f;
        public Transform PositionSource;

        [Header("Input Parameters")]
        [SerializeField] private bool _isPassword = false;
        [SerializeField] private UIBlock _background;
        [SerializeField] private Color _classicColor;
        [SerializeField] private Color _hoverColor;

        private string _placeholder = "";
        private string _text = "";
        public string Text => _text;

        private void Awake()
        {
            _placeholder = TextBlock.text;
        }

        private void OnEnable()
        {
            GameEventSystem.Client.Register<UIPointerEvent>(OnUIPointerEvent);
        }

        private void OnDisable()
        {
            GameEventSystem.Client.Unregister<UIPointerEvent>(OnUIPointerEvent);
        }

        /// <summary>
        /// Handles UI pointer events from the client event bus.
        /// </summary>
        private void OnUIPointerEvent(UIPointerEvent e)
        {
            if (e.Target != this.gameObject)
            {
                _background.Color = _classicColor;
                return;
            }

            switch (e.State)
            {
                case UIInteractionState.HoverEnter:
                    _background.Color = _hoverColor;
                    break;
                case UIInteractionState.HoverExit:
                    _background.Color = _classicColor;
                    break;
                case UIInteractionState.Select:
                    OpenKeyboard();
                    break;
            }
        }

        /// <summary>
        /// Opens the non-native keyboard for text input.
        /// </summary>
        public void OpenKeyboard()
        {
            if (!this.enabled) return;
            NonNativeKeyboard.Instance.InputField.onValueChanged.RemoveAllListeners();
            NonNativeKeyboard.Instance.InputField.text = TextBlock.text == _placeholder ? "" : _text;
            NonNativeKeyboard.Instance.InputField.onValueChanged.AddListener(x =>
            {
                if (x.Length <= 0) { TextBlock.text = _placeholder; }
                else
                {
                    TextBlock.text = _isPassword ? new string('*', x.Length) : x;
                    _text = x;
                }
            });
            NonNativeKeyboard.Instance.PresentKeyboard();
            Vector3 direction = PositionSource.forward;
            direction.y = 0;
            direction.Normalize();
            Vector3 targetPosition = PositionSource.position + direction * Distance + Vector3.up * VerticalOffset;
            NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);
        }
    }
}