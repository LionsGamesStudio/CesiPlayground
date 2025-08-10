using UnityEngine;

namespace CesiPlayground.Client.XR.Hands
{
    /// <summary>
    /// A data container component holding references for a hand model's parts.
    /// Used for procedural posing and animation.
    /// </summary>
    public class HandData : MonoBehaviour
    {
        public enum HandModelType { Left, Right }

        public HandModelType handType;
        public Transform root;
        public Animator animator;
        public Transform[] fingerBones;
    }
}