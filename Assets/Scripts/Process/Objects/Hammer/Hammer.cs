using Assets.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Process.Object.Hammer
{ 
    public class Hammer : GrabbableObject
    {
        [Header("Hit Zone")]
        public Collider RightZoneCollider;
        public Collider LeftZoneCollider;

        public void Start()
        {
            if (RightZoneCollider == null || LeftZoneCollider == null) return;

            RightZoneCollider.isTrigger = true;
            LeftZoneCollider.isTrigger = true;

            RightZoneCollider.gameObject.tag = "HammerHitZone";
            LeftZoneCollider.gameObject.tag = "HammerHitZone";

        }
    }
}