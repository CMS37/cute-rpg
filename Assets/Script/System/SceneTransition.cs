// Assets/Scripts/System/SceneTransition.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game.System
{
    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition Instance { get; private set; }

        [Header("Fade Settings")]
        [Tooltip("페이드 인/아웃에 걸리는 시간 (초)")]
        [SerializeField] private float fadeDuration = 0.8f;

        // 페이드에 사용할 전면 캔버스 이미지
        private Image fadeImage;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                CreateFadeUI();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void CreateFadeUI()
        {
            // 페이드용 캔버스 생성 및 DontDestroyOnLoad 처리
            var canvasGO = new GameObject("FadeCanvas");
            canvasGO.transform.SetParent(transform, false);

            // UI 루트를 위한 RectTransform 추가
            var canvasRect = canvasGO.AddComponent<RectTransform>();
            canvasRect.anchorMin = Vector2.zero;
            canvasRect.anchorMax = Vector2.one;
            canvasRect.offsetMin = Vector2.zero;
            canvasRect.offsetMax = Vector2.zero;

            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // 페이드 이미지 생성
            var imageGO = new GameObject("FadeImage");
            imageGO.transform.SetParent(canvasGO.transform, false);
            fadeImage = imageGO.AddComponent<Image>();
            fadeImage.color = new Color(0f, 0f, 0f, 0f);

            var rt = fadeImage.rectTransform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        public void FadeToScene(string sceneName)
        {
            Debug.Log($"[SceneTransition] FadeToScene called for scene: {sceneName}");
            if (fadeImage == null)
            {
                Debug.LogError("[SceneTransition] fadeImage가 없습니다.");
                SceneManager.LoadScene(sceneName);
                return;
            }
            StartCoroutine(FadeRoutine(sceneName));
        }

        private IEnumerator FadeRoutine(string sceneName)
        {
            yield return Fade(0f, 1f);
            SceneManager.LoadScene(sceneName);
            yield return null;
            yield return new WaitForSeconds(0.2f);
            yield return Fade(1f, 0f);
        }

        private IEnumerator Fade(float from, float to)
        {
            float elapsed = 0f;
            var color = fadeImage.color;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Lerp(from, to, elapsed / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
            color.a = to;
            fadeImage.color = color;
            yield break;
        }
    }
}
