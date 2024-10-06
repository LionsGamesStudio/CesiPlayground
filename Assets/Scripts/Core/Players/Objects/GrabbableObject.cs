using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts.Core.Players.Objects
{
    [RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody), typeof(Collider))]
    public abstract class GrabbableObject : MonoBehaviour
    {

    }
}