using UnityEngine;
using UnityEngine.Tilemaps;
using Game.Managers;
using Game.Actors;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CharacterStats))]
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [Header("이동 속도")]
        [SerializeField] private float moveSpeed = 7f;

        [Header("맵 경계 Tilemap")]
        [SerializeField] private Tilemap groundTilemap;

        [Header("전투 세팅")]
        [SerializeField] private float attackRange    = 1.2f;
        [SerializeField] private float attackCooldown = 0.5f;

        private Rigidbody2D rb;
        private Animator     animator;
        private Vector2      movement;
        private float        ppu = 16f;

        private Vector2 minBounds;
        private Vector2 maxBounds;

        private InputManager   inputManager;
        private UIManager      uiManager;
        private CharacterStats stats;
        private float          nextAttackTime;
        private bool           facingRight = true;
        private bool           isDead = false;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            rb       = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            stats    = GetComponent<CharacterStats>();
            inputManager = GameManager.Instance.InputManager;
            uiManager = GameManager.Instance.UIManager;
            stats.OnDeath += HandleDeath;
        }

        private void Start()
        {
            if (groundTilemap == null)
            {
                Debug.LogError("[PlayerController] groundTilemap이 할당되지 않았습니다.");
                return;
            }

            var local    = groundTilemap.localBounds;
            var pos      = groundTilemap.transform.position;
            var worldMin = local.min + pos;
            var worldMax = local.max + pos;

            var cell    = groundTilemap.cellSize;
            float shrinkX = cell.x + 0.5f;
            float shrinkY = cell.y;

            minBounds = new Vector2(worldMin.x + shrinkX, worldMin.y + shrinkY);
            maxBounds = new Vector2(worldMax.x - shrinkX, worldMax.y - shrinkY);
        }

        private void Update()
        {
            if (isDead) return;

            movement = inputManager.GetMovement();
            float speed = Mathf.Abs(movement.x) + Mathf.Abs(movement.y);
            animator.SetFloat("Speed", speed);

            if (movement.x > 0 && !facingRight) Flip();
            else if (movement.x < 0 && facingRight) Flip();

            if (Time.time >= nextAttackTime && inputManager.GetAttackInput())
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        private void FixedUpdate()
        {
            if (isDead) return;

            var raw  = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
            var snap = new Vector2(
                Mathf.Round(raw.x * ppu) / ppu,
                Mathf.Round(raw.y * ppu) / ppu);

            snap.x = Mathf.Clamp(snap.x, minBounds.x, maxBounds.x);
            snap.y = Mathf.Clamp(snap.y, minBounds.y, maxBounds.y);
            rb.MovePosition(snap);
        }

        private void Attack()
        {
            animator.SetTrigger("Attack");

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<MonsterBase>(out var monster))
                {
                    var monsterStats = monster.GetComponent<CharacterStats>();
                    if (monsterStats != null)
                        monsterStats.TakeDamage(stats.attack.Current);
                }
            }
        }

        private void Flip()
        {
            if (isDead) return;

            facingRight = !facingRight;
            var s       = transform.localScale;
            s.x *= -1;
            transform.localScale = s;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        private void HandleDeath()
        {
            if (isDead) return;
            isDead = true;

            animator.SetTrigger("Die");
            Debug.Log("player died");

            inputManager.LockInput();

        }

        public void OnDeathAnimationEnd()
        {
            //사망시 띄울 UI -> 지금은 일단 일시정지 메뉴를 띄움
            uiManager.TogglePauseMenu(true);
        }
    }
}
