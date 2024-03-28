using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    /// <summary>
    /// Create a keyboard when click on TMP_Input. To Place on TMP_Input.
    /// </summary>
    public class ShowKeyboardUI : MonoBehaviour
    {
        private TMP_InputField inputField;

        public float Distance = 0.5f;
        public float VerticalOffset = -0.5f;

        public Transform PositionSource;

        private void Start()
        {
            inputField = GetComponent<TMP_InputField>();

            inputField.onSelect.AddListener(x => OpenKeyboard());
        }

        public void OpenKeyboard()
        {
            NonNativeKeyboard.Instance.InputField = inputField;
            NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);

            Vector3 direction = PositionSource.forward;
            direction.y = 0;
            direction.Normalize();

            Vector3 targetPosition = PositionSource.position + direction * Distance + Vector3.up * VerticalOffset;

            NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);
        }
    }
}
