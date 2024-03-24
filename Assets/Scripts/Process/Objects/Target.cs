using Assets.Scripts.Core;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Game;
using Assets.Scripts.Core.Spawn.Spawners;
using Assets.Scripts.Process.Events;
using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Process.Object
{
    [RequireComponent(typeof(Collider))]
    public class Target : MonoBehaviour
    {
        public Game game;
        public int score;

        public GameObject effectOnDestroy;


        public void Awake()
        {
            // Target Layer
            gameObject.layer = 9;

        }

        public void PlayEffect(GameObject effect)
        {
             GameObject.Instantiate(effect, transform.position, transform.rotation);
        }

        public void IsHit(PlayerData playerData)
        {
            OnScoreEvent scoreEvent = new OnScoreEvent();

            scoreEvent.Points = score;
            scoreEvent.PlayerData = playerData;
            scoreEvent.GameId = game.Data.GameId;

            EventBus<OnScoreEvent>.Raise(scoreEvent);
        }

    }

}