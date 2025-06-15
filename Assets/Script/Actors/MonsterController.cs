using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Player;

namespace Game.Actors
{
    [RequireComponent(typeof(CharacterStats), typeof(Animator))]
    public class MonsterController : MonoBehaviour
    {
        private Transform      playerTransform;
        private CharacterStats playerStats;

        [Header("몬스터 AI 및 전투 설정")]
        [SerializeField] private float agroRange      = 5f;
        [SerializeField] private float chaseSpeed     = 2f;
        [SerializeField] private float attackRange    = 1.5f;
        [SerializeField] private float attackCooldown = 2f;

        private CharacterStats stats;
        private Animator       animator;
        private float          nextAttackTime;
        private bool           isDead;

        private void Awake()
        {
            BindPlayer();
            SceneManager.sceneLoaded += OnSceneLoaded;

            stats    = GetComponent<CharacterStats>();
            animator = GetComponent<Animator>();
            stats.OnDamaged += _ => animator.SetTrigger("Hit");
            stats.OnDeath   += HandleDeath;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene s, LoadSceneMode m)
        {   
            BindPlayer();
        }

        private void BindPlayer()
        {
            var pc = PlayerController.Instance;
            if (pc != null)
            {
                playerTransform = pc.transform;
                playerStats     = pc.GetComponent<CharacterStats>();
                enabled = true;
            }
            else
            {
                playerTransform = null;
                playerStats     = null;
                enabled = false;
            }
        }

        private void Update()
        {
            if (isDead) return;

            float dist = Vector2.Distance(transform.position, playerTransform.position);

            if (dist <= agroRange && dist > attackRange)
            {
                animator.SetFloat("Speed", 1f);
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    playerTransform.position,
                    chaseSpeed * Time.deltaTime
                );
            }
            else animator.SetFloat("Speed", 0f);

            if (dist <= attackRange && Time.time >= nextAttackTime)
            {
                int raw = stats.attack.Current;
                int dmg = Mathf.Max(0, raw - playerStats.defense.Current);
                Debug.Log($"[Monster] rawAttack={raw}, playerDef={playerStats.defense.Current}, dmg={dmg}");
                playerStats.TakeDamage(stats.attack.Current);
                nextAttackTime = Time.time + attackCooldown;
            }
        }

        private void HandleDeath()
        {
            if (isDead) return;
            isDead = true;

            animator.SetTrigger("Die");
            var col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            enabled = false;
            Destroy(gameObject, 1f);
        }

        //범위 확인 테스트용
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, agroRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
