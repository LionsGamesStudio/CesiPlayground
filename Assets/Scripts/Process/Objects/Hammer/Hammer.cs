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

        public List<LayerMask> LayersToCollide = new List<LayerMask>();

        public void Start()
        {
            if (RightZoneCollider == null || LeftZoneCollider == null) return;

            RightZoneCollider.isTrigger = true;
            LeftZoneCollider.isTrigger = true;

            TriggerListener rightTriggerListener = RightZoneCollider.AddComponent<TriggerListener>();
            TriggerListener leftTriggerListener = LeftZoneCollider.AddComponent<TriggerListener>();

            rightTriggerListener.onTriggerEnter += DetectCollide;
            leftTriggerListener.onTriggerEnter += DetectCollide;

        }

        private void DetectCollide(Collider other, GameObject o)
        {
            bool hasHit = false;

            // Check if hit
            foreach (LayerMask layerMask in LayersToCollide)
            {
                if (layerMask == (layerMask | (1 << other.gameObject.layer)))
                {
                    hasHit = true; break;
                }
            }

            if(hasHit)
            {
                Target target = other.gameObject.GetComponent<Target>();

                if (target != null)
                {
                    //target.IsHit("Test");
                }
            }
        }
    }
}