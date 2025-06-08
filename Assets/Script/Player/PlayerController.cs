using UnityEngine;
using UnityEngine.Tilemaps;
using Game.Managers;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("이동 속도")]
        [SerializeField] private float moveSpeed = 7f;

        [Header("맵 경계 Tilemap")]
        [SerializeField] private Tilemap groundTilemap;

        private Rigidbody2D rb;
        private Animator animator;
        private Vector2 movement;
        private float ppu = 16f;

        private Vector2 minBounds;
        private Vector2 maxBounds;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (groundTilemap == null)
            {
                Debug.LogError("[PlayerController] groundTilemap이 할당되지 않았습니다.");
                return;
            }

            var local = groundTilemap.localBounds;
            var pos   = groundTilemap.transform.position;
            var worldMin = local.min + pos;
            var worldMax = local.max + pos;

            var cell = groundTilemap.cellSize;
            float shrinkX = cell.x + 0.5f;
            float shrinkY = cell.y;

            minBounds = new Vector2(worldMin.x + shrinkX, worldMin.y + shrinkY);
            maxBounds = new Vector2(worldMax.x - shrinkX, worldMax.y - shrinkY);
        }

        private void Update()
        {
            movement = GameManager.Instance.InputManager.GetMovement();
            animator.SetFloat("Speed", Mathf.Abs(movement.x) + Mathf.Abs(movement.y));
        }

        private void FixedUpdate()
        {
            var raw = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            var snap = new Vector2(
                Mathf.Round(raw.x * ppu) / ppu,
                Mathf.Round(raw.y * ppu) / ppu);

            float x = Mathf.Clamp(snap.x, minBounds.x, maxBounds.x);
            float y = Mathf.Clamp(snap.y, minBounds.y, maxBounds.y);

            rb.MovePosition(new Vector2(x, y));
        }
    }
}
