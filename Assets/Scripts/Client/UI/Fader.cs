using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CesiPlayground.Core;
using UnityEngine.SceneManagement;

namespace CesiPlayground.Client.UI
{
    /// <summary>
    /// A client-side service that provides fade-to-black and fade-from-black functionality.
    /// Essential for smooth, comfortable transitions in VR.
    /// </summary>
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public class Fader : Singleton<Fader>
    {
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            if (this.enabled == false) return;

            // Register this instance with the Service Locator.
            ServiceLocator.Register(this);
            
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
            DontDestroyOnLoad(this.gameObject);
            
            // Subscribe to the sceneLoaded event to automatically find the new camera.
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Camera.main != null)
            {
                _canvas.worldCamera = Camera.main;
            }
        }

        public Coroutine FadeOut(float duration)
        {
            return StartCoroutine(FadeRoutine(1f, duration));
        }

        public Coroutine FadeIn(float duration)
        {
            return StartCoroutine(FadeRoutine(0f, duration));
        }

        private IEnumerator FadeRoutine(float targetAlpha, float duration)
        {
            // Make the fader interactable during fade out, and non-interactable during fade in.
            _canvasGroup.blocksRaycasts = (targetAlpha >= 1f);
            
            float startAlpha = _canvasGroup.alpha;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
                yield return null;
            }

            _canvasGroup.alpha = targetAlpha;
            _canvasGroup.blocksRaycasts = (targetAlpha >= 1f);
        }
    }
}