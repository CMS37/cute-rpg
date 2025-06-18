using UnityEngine;
using Game.Interfaces;
using Game.System;
using Game.Player;
using UnityEngine.Tilemaps;

namespace Game.Actors
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CharacterStats))]
    public class MonsterController : MonoBehaviour
    {
        [Header("AI 세팅")]
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float detectRange = 5f;
        [SerializeField] private float attackRange = 1.2f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private Tilemap groundTilemap;

        public Animator animator { get; private set; }
        public StateMachine StateMachine { get; private set; }
        public IState IdleState { get; private set; }
        public IState MoveState { get; private set; }
        public IState DeadState { get; private set; }
        public IState AttackState { get; private set; }
        public IState HitState { get; private set; }

        public float MoveSpeed => moveSpeed;
        public float AttackCooldown => attackCooldown;

        private Rigidbody2D rb;
        private CharacterStats stats;
        private Transform player;
        public bool isDead { get; private set; }

        private Vector2 minBounds, maxBounds;
        private float ppu = 16f;
        private Vector2? nextPosition = null;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            stats = GetComponent<CharacterStats>();
            stats.OnDeath += OnDeath;
            stats.OnDamaged += OnDamaged;

            StateMachine = new StateMachine();
            IdleState = new IdleState(this);
            MoveState = new MoveState(this);
            DeadState = new DeadState(this);
            AttackState = new AttackState(this);
            HitState = new HitState(this);
        }

        private void Start()
        {
            TryFindPlayer();

            if (groundTilemap != null)
            {
                var local = groundTilemap.localBounds;
                var pos = groundTilemap.transform.position;
                var worldMin = local.min + pos;
                var worldMax = local.max + pos;
                var cell = groundTilemap.cellSize;
                float shrinkX = cell.x + 0.5f;
                float shrinkY = cell.y;
                minBounds = new Vector2(worldMin.x + shrinkX, worldMin.y + shrinkY);
                maxBounds = new Vector2(worldMax.x - shrinkX, worldMax.y - shrinkY);
            }
            Debug.Log($"minBounds: {minBounds}, maxBounds: {maxBounds}");
            StateMachine.ChangeState(IdleState);
        }

        private void Update()
        {
            if (player == null)
                TryFindPlayer();
            StateMachine.Update();
        }

        private void TryFindPlayer()
        {
            var playerObj = FindObjectOfType<PlayerController>();
            if (playerObj != null)
                player = playerObj.transform;
        }
        public void ChasePlayer()
        {
            if (player == null || isDead) return;
        
            Vector2 dir = (player.position - transform.position).normalized;
            Vector2 raw = rb.position + dir * moveSpeed * Time.deltaTime;
            Vector2 snap = new Vector2(
                Mathf.Round(raw.x * ppu) / ppu,
                Mathf.Round(raw.y * ppu) / ppu);

            snap.x = Mathf.Clamp(snap.x, minBounds.x, maxBounds.x);
            snap.y = Mathf.Clamp(snap.y, minBounds.y, maxBounds.y);

            Debug.Log($"dir: {dir}, player: {player.position}, slime: {transform.position}");
            nextPosition = snap;
            animator.SetFloat("Speed", MoveSpeed);

            if (dir.x > 0 && transform.localScale.x < 0)
                Flip();
            else if (dir.x < 0 && transform.localScale.x > 0)
                Flip();
        }

        private void FixedUpdate()
        {
            if (nextPosition.HasValue)
            {
                rb.MovePosition(nextPosition.Value);
                nextPosition = null;
            }
        }

        public void Attack()
        {
            if (player == null || isDead) return;
            var playerStats = player.GetComponent<CharacterStats>();
            if (playerStats != null)
                playerStats.TakeDamage(stats.attack.Current);
        }

        public void Flip()
        {
            if (isDead) return;
            var s = transform.localScale;
            s.x *= -1;
            transform.localScale = s;
        }

        public bool CanSeePlayer()
        {
            if (player == null || isDead) return false;
            float dist = Vector2.Distance(transform.position, player.position);
            return dist <= detectRange;
        }

        public bool InAttackRange()
        {
            if (player == null || isDead) return false;
            float dist = Vector2.Distance(transform.position, player.position);
            return dist <= attackRange;
        }

        private void OnDamaged(int dmg)
        {
            if (isDead) return;
            StateMachine.ChangeState(HitState);
        }

        private void OnDeath()
        {
            if (isDead) return;
            isDead = true;
            StateMachine.ChangeState(DeadState);
        }
    }
}
