using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Game.Player;

namespace Managers
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [Header("팔로우 대상")]
        [Tooltip("따라갈 대상 Transform")]
        [SerializeField] private Transform target;

        [Header("오프셋")]
        [Tooltip("타겟 위치에서 카메라가 떨어질 거리")]
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

        [Header("경계 추출할 Tilemap")]
        [SerializeField] private Tilemap groundTilemap;

        private Camera cam;
        private float halfHeight;
        private float halfWidth;
        private Vector2 minBounds;
        private Vector2 maxBounds;
        private float smoothSpeed = 0.125f;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            halfHeight = cam.orthographicSize;
            halfWidth  = halfHeight * cam.aspect;

            // 초기 타겟 설정: PlayerController.Instance
            if (target == null && PlayerController.Instance != null)
                target = PlayerController.Instance.transform;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            if (groundTilemap == null)
            {
                Debug.LogError("CameraFollow: groundTilemap이 할당되지 않았습니다.");
                return;
            }

            var localBounds = groundTilemap.localBounds;
            var tilemapPos  = groundTilemap.transform.position;
            var worldMin    = localBounds.min + tilemapPos;
            var worldMax    = localBounds.max + tilemapPos;
            var cellSize    = groundTilemap.cellSize;

            float extraShrinkX = 0.5f;
            float shrinkX      = cellSize.x + extraShrinkX;
            float shrinkY      = cellSize.y;

            minBounds = new Vector2(worldMin.x + shrinkX, worldMin.y + shrinkY);
            maxBounds = new Vector2(worldMax.x - shrinkX, worldMax.y - shrinkY);
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            var desiredPos  = target.position + offset;
            var smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);

            float clampedX = Mathf.Clamp(smoothedPos.x,
                minBounds.x + halfWidth,
                maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(smoothedPos.y,
                minBounds.y + halfHeight,
                maxBounds.y - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, smoothedPos.z);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (PlayerController.Instance != null)
                target = PlayerController.Instance.transform;
        }

        public void SetTarget(Transform t)
        {
            target = t;
        }
    }
}
