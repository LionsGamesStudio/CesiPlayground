using Assets.Scripts.Core.Events;
using Assets.Scripts.Process.Events.Objects.MollEvent;
using Assets.Scripts.Process.Object;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Process.Objects.Targets
{
    public class Moll : Target
    {
        [Header("Data")]
        public int Damage;
        public float TimeOutside;
        public float TimeAlive;

        public Collider HitZone;

        [Header("Effect")]
        public GameObject EffectOnDying;
        public float YToOutside;

        private float _timeAlreadyPassedOutside = 0;
        private float _timeAlreadyPassedAlive = 0;

        private float _multiplier = 1;
        private int _outsideRate;

        private EventBinding<OnMollBirth> _onMollBirth;

        public override void Awake()
        {
            base.Awake();
            _onMollBirth = new EventBinding<OnMollBirth>(OnMollBirth);
            EventBus<OnMollBirth>.Register(_onMollBirth);
        }

        /// <summary>
        /// Logic of the moll
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Loop()
        {
            while(true)
            {
                _timeAlreadyPassedAlive += Time.deltaTime;

                if (IsOutside)
                    _timeAlreadyPassedOutside += Time.deltaTime;

                if (_timeAlreadyPassedOutside >= TimeOutside * _multiplier && IsOutside)
                {
                    IsOutside = false;
                    yield return StartCoroutine(GoInside());
                }

                // Determine if go outside or not
                float outsideRatePick = Random.Range(0, 100);
                bool goOutside = outsideRatePick < _outsideRate * _multiplier;

                if (goOutside && !IsOutside)
                {
                    IsOutside = true;
                    _timeAlreadyPassedOutside = 0;
                    yield return StartCoroutine(GoOutside());
                }

                if (_timeAlreadyPassedAlive >= TimeAlive * _multiplier && !IsOutside)
                {
                    OnMollDying onMollDying = new OnMollDying();
                    onMollDying.Game = this.game;
                    onMollDying.Moll = gameObject;
                    onMollDying.Damage = Damage / _multiplier;

                    EventBus<OnMollDying>.Raise(onMollDying);

                    StopCoroutine(Loop());
                    break;
                }
            }
        }

        #region Positioning

        /// <summary>
        /// Make the moll go outside
        /// </summary>
        /// <returns></returns>
        private IEnumerator GoOutside()
        {
            transform.position += new Vector3(0, YToOutside, 0);
            HitZone.enabled = true;

            yield return new WaitForSeconds(1);
        }

        /// <summary>
        /// Make the moll go back inside
        /// </summary>
        /// <returns></returns>
        private IEnumerator GoInside()
        {
            transform.position -= new Vector3(0, YToOutside, 0);
            HitZone.enabled = false;

            yield return new WaitForSeconds(1);
        }

        #endregion

        /// <summary>
        /// Event to get data on birth
        /// </summary>
        /// <param name="e"></param>
        private void OnMollBirth(OnMollBirth e)
        {
            if(e.IdMoll == ID)
            {
                _multiplier = e.Multiplier;
                _outsideRate = e.OutsideRate;
                game = e.Game;

                StartCoroutine(Loop());
            }
        }

        public bool IsOutside
        {
            get; set;
        }
    }
}
