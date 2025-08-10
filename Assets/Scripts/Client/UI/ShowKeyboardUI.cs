using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine;

namespace CesiPlayground.Client.UI
{
    /// <summary>
    /// A client-side utility that opens the MRTK NonNativeKeyboard
    /// when a TMP_InputField is selected.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class ShowKeyboardUI : MonoBehaviour
    {
        [Tooltip("The transform used to position the keyboard in front of the player.")]
        [SerializeField] private Transform positionSource;
        
        [Tooltip("How far in front of the position source to place the keyboard.")]
        [SerializeField] private float distance = 0.5f;
        
        [Tooltip("The vertical offset from the position source.")]
        [SerializeField] private float verticalOffset = -0.5f;

        private TMP_InputField _inputField;

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onSelect.AddListener(x => OpenKeyboard());

            if (positionSource == null)
            {
                // Default to the main camera if no source is provided.
                positionSource = Camera.main.transform;
            }
        }

        public void OpenKeyboard()
        {
            NonNativeKeyboard.Instance.InputField = _inputField;
            NonNativeKeyboard.Instance.PresentKeyboard(_inputField.text);

            if (positionSource == null) return;
            
            Vector3 direction = positionSource.forward;
            direction.y = 0;
            direction.Normalize();

            Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;

            NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);
        }
    }
}