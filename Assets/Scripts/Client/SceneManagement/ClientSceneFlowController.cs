using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Client.Players;
using CesiPlayground.Client.World;
using CesiPlayground.Client.UI;
using CesiPlayground.Client.Network;
using CesiPlayground.Shared.Network;

namespace CesiPlayground.Client.SceneManagement
{
    public class ClientSceneFlowController : MonoBehaviour
    {
        [Header("Initial Scene")]
        [SerializeField] private DataScene startMenuScene;
        
        private ScenesManager _scenesManager;
        private Fader _fader;

        private void Start()
        {
            _scenesManager = ServiceLocator.Get<ScenesManager>();
            _fader = ServiceLocator.Get<Fader>();
            GameEventSystem.Client.Register<TransitionToSceneEvent>(OnTransitionRequest);

            if (startMenuScene != null)
            {
                StartCoroutine(InitialLoadRoutine());
            }
        }

        private void OnDestroy()
        {
            GameEventSystem.Client.Unregister<TransitionToSceneEvent>(OnTransitionRequest);
        }
        
        private void OnTransitionRequest(TransitionToSceneEvent e)
        {
            StartCoroutine(TransitionRoutine(e));
        }

        /// <summary>
        /// A coroutine specifically for the very first scene load.
        /// It ensures the game starts with a fade-in.
        /// </summary>
        private IEnumerator InitialLoadRoutine()
        {
            // Ensure the screen is black before we do anything.
            // A duration of 0 makes the fade instantaneous.
            yield return _fader.FadeOut(0f);
            
            // Load the start menu scene additively.
            _scenesManager.LoadScene(startMenuScene);
            yield return new WaitUntil(() => startMenuScene.IsLoaded);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(startMenuScene.SceneName));
            
            // Now that the scene is loaded and ready behind the black screen, fade in.
            yield return _fader.FadeIn(1f);
        }

        private IEnumerator TransitionRoutine(TransitionToSceneEvent e)
        {
            yield return _fader.FadeOut(0.5f);

            if (e.SceneToUnload != null)
            {
                _scenesManager.UnloadScene(e.SceneToUnload);
                yield return new WaitUntil(() => !e.SceneToUnload.IsLoaded);
            }

            if (e.SceneToLoad != null)
            {
                _scenesManager.LoadScene(e.SceneToLoad);
                yield return new WaitUntil(() => e.SceneToLoad.IsLoaded);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(e.SceneToLoad.SceneName));
            }

            // Wait for one frame to allow the new scene's physics to initialize
            // before we try to find objects or teleport the player.
            yield return new WaitForEndOfFrame();

            GateTrigger destinationGate = FindDestinationGate(e.DestinationGateName);
            if (destinationGate != null && PlayerRigController.HasInstance)
            {
                PlayerRigController.TryGetInstance().TeleportTo(destinationGate.transform);

                // Notify the server that we have arrived at our new authoritative position.
                var networkManager = ServiceLocator.Get<ClientNetworkManager>();
                networkManager?.Send(new ClientReadyInSceneMessage
                {
                    Position = destinationGate.transform.position,
                    Rotation = destinationGate.transform.rotation
                });
            }
            
            yield return _fader.FadeIn(0.5f);
        }

        private GateTrigger FindDestinationGate(string gateName)
        {
            if (string.IsNullOrEmpty(gateName)) return null;
            var allGates = FindObjectsByType<GateTrigger>(FindObjectsSortMode.None);
            foreach (var gate in allGates)
            {
                if (gate.GateName == gateName) return gate;
            }
            return null;
        }
    }
}