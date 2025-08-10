using UnityEngine;

namespace CesiPlayground.Core.Utilities
{
    /// <summary>
    /// A simple utility component that calls DontDestroyOnLoad on its GameObject.
    /// Useful for creating persistent manager objects.
    /// </summary>
    public class DontDestroyBetweenScene : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}