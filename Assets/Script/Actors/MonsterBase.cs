using UnityEngine;
using Game.Interfaces;
using Game.System;
using Game.Player;
using UnityEngine.Tilemaps;

namespace Game.Actors
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CharacterStats))]
    public abstract class MonsterBase : MonoBehaviour
    {
        [Header("AI 세팅")]
        [SerializeField] protected float moveSpeed = 3f;
        [SerializeField] protected float detectRange = 5f;
        [SerializeField] protected float attackRange = 1.2f;
        [SerializeField] protected float attackCooldown = 1f;
        [SerializeField] protected float skillCooldown = 5f;
        [SerializeField] protected Tilemap groundTilemap;
        // [SerializeField] protected DropTable dropTable;

        public Animator animator { get; protected set; }
        public StateMachine StateMachine { get; protected set; }
        public IState IdleState { get; protected set; }
        public IState MoveState { get; protected set; }
        public IState AttackState { get; protected set; }
        public IState HitState { get; protected set; }
        public IState DeadState { get; protected set; }
        public IState SkillState { get; protected set; }

        protected Rigidbody2D rb;
        protected CharacterStats stats;
        protected Transform player;
        protected float skillTimer = 0f;
        public bool isDead { get; protected set; }

        protected Vector2 minBounds, maxBounds;
        protected float ppu = 16f;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            stats = GetComponent<CharacterStats>();
            stats.OnDeath += OnDeath;
            stats.OnDamaged += OnDamaged;
        }

        protected virtual void Start()
        {
            var playerObj = FindObjectOfType<PlayerController>();
            if (playerObj != null)
                player = playerObj.transform;

            if (groundTilemap != null)
            {
                var local = groundTilemap.localBounds;
                var pos = groundTilemap.transform.position;
                var worldMin = local.min + pos;
                var worldMax = local.max + pos;
                var cell = groundTilemap.cellSize;
                float shrinkX = cell.x * 0.5f;
                float shrinkY = cell.y * 0.5f;
                minBounds = new Vector2(worldMin.x + shrinkX, worldMin.y + shrinkY);
                maxBounds = new Vector2(worldMax.x - shrinkX, worldMax.y - shrinkY);
            }
        }

        protected virtual void Update()
        {
            StateMachine?.Update();
            skillTimer += Time.deltaTime;
        }

        protected virtual void FixedUpdate()
        {
            StateMachine?.FixedUpdate();
        }

        public virtual void ChasePlayer()
        {
            if (player == null || isDead) return;

            Vector2 dir = (player.position - transform.position).normalized;
            Vector2 raw = rb.position + dir * moveSpeed * Time.deltaTime;
            Vector2 snap = new Vector2(
                Mathf.Round(raw.x * ppu) / ppu,
                Mathf.Round(raw.y * ppu) / ppu);

            snap.x = Mathf.Clamp(snap.x, minBounds.x, maxBounds.x);
            snap.y = Mathf.Clamp(snap.y, minBounds.y, maxBounds.y);

            rb.MovePosition(snap);
            animator.SetFloat("Speed", moveSpeed);

            if (dir.x > 0 && transform.localScale.x < 0)
                Flip();
            else if (dir.x < 0 && transform.localScale.x > 0)
                Flip();
        }

        public virtual void Attack()
        {
            DealDamageToPlayer();
        }

        public virtual void UseSkill() {}

        public virtual void DealDamageToPlayer(float multiplier = 1f)
        {
            if (player == null) return;
            var playerStats = player.GetComponent<CharacterStats>();
            if (playerStats != null)
                playerStats.TakeDamage(Mathf.RoundToInt(stats.attack.Current * multiplier));
        }

        public virtual void Flip()
        {
            if (isDead) return;
            var s = transform.localScale;
            s.x *= -1;
            transform.localScale = s;
        }

        public virtual bool CanSeePlayer()
        {
            if (player == null || isDead) return false;
            float dist = Vector2.Distance(transform.position, player.position);
            return dist <= detectRange;
        }

        public virtual bool InAttackRange()
        {
            if (player == null || isDead) return false;
            float dist = Vector2.Distance(transform.position, player.position);
            return dist <= attackRange;
        }

        protected virtual void OnDamaged(int dmg)
        {
            if (isDead) return;
            StateMachine?.ChangeState(HitState);
        }

        protected virtual void OnDeath()
        {
            if (isDead) return;
            isDead = true;
            StateMachine?.ChangeState(DeadState);
        }

        public float MoveSpeed => moveSpeed;
        public float AttackCooldown => attackCooldown;
        public float SkillCooldown => skillCooldown;
        public float SkillTimer
        {
            get => skillTimer;
            set => skillTimer = value;
        }
        // public DropTable DropTable => dropTable;
    }
}