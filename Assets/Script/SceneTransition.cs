using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Managers;

namespace Managers
{

    public class SceneTransition : MonoBehaviour
    {
        public static SceneTransition Instance { get; private set; }

        [Header("페이드용 이미지")]
        public Image fadeImage;

        [Tooltip("페이드 인/아웃에 걸리는 시간 (초)")]
        public float fadeDuration = 0.8f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                if (fadeImage != null)
                {
                    DontDestroyOnLoad(fadeImage.transform.parent.gameObject);

                    Color c = fadeImage.color;
                    c.a = 0f;
                    fadeImage.color = c;
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void FadeToScene(string sceneName)
        {
            if (fadeImage == null)
            {
                Debug.LogError("SceneTransition: fadeImage가 할당되지 않았습니다!");
                SceneManager.LoadScene(sceneName);
                return;
            }

            StartCoroutine(FadeRoutine(sceneName));
        }

        private IEnumerator FadeRoutine(string sceneName)
        {
            float t = 0f;
            Color color = fadeImage.color;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
            color.a = 1f;
            fadeImage.color = color;

            SceneManager.LoadScene(sceneName);
            yield return null;
            yield return new WaitForSeconds(0.2f);

            t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
            color.a = 0f;
            fadeImage.color = color;
        }
    }
}
