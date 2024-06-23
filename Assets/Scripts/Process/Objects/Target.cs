using Assets.Scripts.Core;
using Assets.Scripts.Core.AI.BehaviourTrees;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Game;
using Assets.Scripts.Core.Spawn.Spawners;
using Assets.Scripts.Process.Events;
using Assets.Scripts.Utilities.Sound;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Process.Object
{
    [RequireComponent(typeof(Collider))]
    public class Target : MonoBehaviour
    {
        public Game game;
        public float score;

        public List<string> tagHitting = new List<string>();
        public GameObject effectOnDestroy;

        private int HASH_ID;


        public virtual void Awake()
        {
            // Target Layer
            gameObject.layer = 9;
            HASH_ID = Guid.NewGuid().GetHashCode();
        }

        public int ID { get { return HASH_ID; } }


        /// <summary>
        /// Detect if was hit
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (!tagHitting.Contains(other.tag)) return;

            IsHit();

            // Play effect on destroy
            GameObject effect = Instantiate(effectOnDestroy);
            effect.transform.parent = transform;
            effect.transform.localPosition = Vector3.zero;
            effect.transform.parent = null;

            if (GetComponent<OnDestroyPlaySound>() != null)
                GetComponent<OnDestroyPlaySound>().OnBeforeDestroy(() =>
                {
                    SendDispawnRequest();
                });
            else
            {
                SendDispawnRequest();
            }
        }

        /// <summary>
        /// Send event to score because target was hit
        /// </summary>
        private void IsHit()
        {
            OnScoreEvent scoreEvent = new OnScoreEvent();

            scoreEvent.Points = score;
            scoreEvent.PlayerData = game.CurrentPlayer.PlayerData;
            scoreEvent.GameId = game.Data.GameId;

            EventBus<OnScoreEvent>.Raise(scoreEvent);
        }

        /// <summary>
        /// Send a request to dispawn the target
        /// </summary>
        private void SendDispawnRequest()
        {
            OnDispawnRequestSend request = new OnDispawnRequestSend();
            request.ObjectToDispawn = gameObject;
            request.Spawner = game.Spawner;

            EventBus<OnDispawnRequestSend>.Raise(request);
        }
    }

}