using Assets.Scripts.Core.AI.BehaviourTrees;
using Assets.Scripts.Core.AI.BehaviourTrees.Nodes;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Process.AI;
using Assets.Scripts.Process.Events.Objects.MollEvent;
using Assets.Scripts.Process.Object;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Process.Objects.Targets
{
    [RequireComponent(typeof(Assets.Scripts.Core.AI.BehaviourTrees.Tree))]
    public class Moll : Target
    {
        [Header("Data")]
        public int Damage;
        public float TimeToLive;
        public float WaitTime;

        public Collider HitZone;

        [Header("Effect")]
        public GameObject EffectOnDying;

        private BaseMollBT _tree;
        private float _timeLived = 0;
        private float _multiplier = 1;
        private Vector3 _spawnPos;

        private EventBinding<OnMollBirth> _onMollBirth;

        public override void Awake()
        {
            base.Awake();
            _onMollBirth = new EventBinding<OnMollBirth>(OnMollBirth);
            EventBus<OnMollBirth>.Register(_onMollBirth);

            _tree = GetComponent<BaseMollBT>();

            // Don't activate the tree until the moll is born
            _tree.enabled = false;
        }

        public void Update()
        {
            _timeLived += Time.deltaTime;
            if (_timeLived >= TimeToLive)
            {
                OnMollDying onMollDying = new OnMollDying();
                onMollDying.Game = this.game;
                onMollDying.Moll = gameObject;
                onMollDying.Damage = Damage * _multiplier;

                EventBus<OnMollDying>.Raise(onMollDying);

                foreach (Transform pos in _tree.WaypointsUsed)
                {
                    if (pos != _tree.SpawnPosition)
                    {
                        Destroy(pos.gameObject);
                    }
                }
            }

            if(transform.position == _spawnPos)
            {
                HitZone.enabled = false;
            }
            else
            {
                HitZone.enabled = true;
            }
        }

        /// <summary>
        /// Event to get data on birth
        /// </summary>
        /// <param name="e"></param>
        private void OnMollBirth(OnMollBirth e)
        {
            if(e.IdMoll == ID)
            {
                game = e.Game;
                _tree.Waypoints = e.Waypoints;
                _tree.SpawnPosition = e.SpawnPosition;
                _tree.Speed *= e.Multiplier;
                _tree.WaitTime = WaitTime * e.Multiplier;
                _tree.enabled = true;

                _multiplier = e.Multiplier;
                score *= e.Multiplier;

                _spawnPos = e.SpawnPosition.position;
            }
        }
    }
}
