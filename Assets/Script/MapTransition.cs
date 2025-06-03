using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(Collider2D))]
    public class MapTransition : MonoBehaviour
    {
        [Tooltip("다음 씬 이름")]
        public string nextSceneName;

        [Tooltip("다음 씬으로 넘어간 뒤 플레이어 위치")]
        public Vector2 spawnPositionInNextScene = Vector2.zero;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            if (string.IsNullOrEmpty(nextSceneName))
            {
                Debug.LogError("MapTransition: nextSceneName이 설정되지 않았습니다!");
                return;
            }

            GameManager.Instance.nextSceneToLoad = nextSceneName;
            GameManager.Instance.nextSpawnPosition = spawnPositionInNextScene;

            SceneTransition.Instance.FadeToScene(nextSceneName);
        }
    }
}
