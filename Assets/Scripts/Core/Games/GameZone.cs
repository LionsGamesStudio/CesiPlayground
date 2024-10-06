using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Events.Players;
using Assets.Scripts.Core.Players;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core.Games
{
    [RequireComponent(typeof(BoxCollider))]
    public class GameZone : MonoBehaviour
    {
        [SerializeField] private Game _game;
        [SerializeField] private Transform _posToSendBackPlayer;

        private bool _isPlayerInZone = false;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                // Player enter the zone and is the one who will play the game
                if (!_isPlayerInZone)
                {
                    EventBus<PlayerEnterGameZoneEvent>.Raise(new PlayerEnterGameZoneEvent()
                    {
                        Player = player,
                        Game = _game
                    });

                    _isPlayerInZone = true;

                    StartCoroutine(GlowEffect(Color.green));

                }
                else // Send back player
                {
                    player.transform.position = _posToSendBackPlayer.position;
                    StartCoroutine(GlowEffect(Color.red));
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                _isPlayerInZone = false;

                EventBus<PlayerLeaveGameZoneEvent>.Raise(new PlayerLeaveGameZoneEvent()
                {
                    Player = player,
                    Game = _game
                });

                StartCoroutine(GlowEffect(Color.blue));
            }
        }

        private IEnumerator GlowEffect(Color c)
        {
            _meshRenderer.enabled = true;
            _meshRenderer.material.color = c;

            yield return new WaitForSeconds(2f);

            _meshRenderer.material.color = Color.white;
            _meshRenderer.enabled = false;
        }
    }
}
