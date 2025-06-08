using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Managers;

namespace Game.World
{
    [RequireComponent(typeof(Collider2D))]
    public class MapTransition : MonoBehaviour
    {
        [Header("Transition Settings")]
        [Tooltip("다음 로드할 씬 이름")]  
        [SerializeField] private string nextSceneName;

        [Tooltip("플레이어가 다음 씬으로 로드된 후 스폰될 위치")]  
        [SerializeField] private Vector2 spawnPositionInNextScene = Vector2.zero;

        private void Reset()
        {
            var col = GetComponent<Collider2D>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            Debug.Log($"[MapTransition] 트리거 활성화: 씬={nextSceneName}, 스폰위치={spawnPositionInNextScene}");

            if (string.IsNullOrEmpty(nextSceneName))
            {
                Debug.LogError("[MapTransition] nextSceneName이 설정되지 않았습니다.");
                return;
            }

            GameManager.Instance.SetNextScene(nextSceneName, spawnPositionInNextScene);
        }
    }
}
