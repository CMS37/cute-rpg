using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Game.Player;

namespace Game.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        [Tooltip("채워지는 바 이미지 (HP_Fill)")]
        [SerializeField] private Image fillImage;

        private CharacterStats playerStats;

        private void Awake()
        {
            if (fillImage == null)
            {
                var fillTransform = transform.Find("HP_Fill");
                if (fillTransform != null)
                    fillImage = fillTransform.GetComponent<Image>();
            }

            if (fillImage == null)
                Debug.LogError("[HealthBarUI] fillImage가 할당되지 않았거나 HP_Fill 자식 오브젝트를 찾지 못했습니다.");
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            BindToPlayer();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            UnbindPlayerEvents();
        }

        private void Start()
        {
            BindToPlayer();
            UpdateHealthBar();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            BindToPlayer();
            UpdateHealthBar();
        }

        private void BindToPlayer()
        {
            UnbindPlayerEvents();

            var pc = PlayerController.Instance;
            if (pc == null)
            {
                playerStats = null;
                return;
            }

            playerStats = pc.GetComponent<CharacterStats>();
            if (playerStats != null)
            {
                playerStats.OnDamaged += OnPlayerDamaged;
                playerStats.OnDeath   += OnPlayerDeath;
            }
        }

        private void UnbindPlayerEvents()
        {
            if (playerStats != null)
            {
                playerStats.OnDamaged -= OnPlayerDamaged;
                playerStats.OnDeath   -= OnPlayerDeath;
            }
        }

        private void OnPlayerDamaged(int dmg)
        {
            UpdateHealthBar();
        }

        private void OnPlayerDeath()
        {
            if (fillImage != null)
                fillImage.fillAmount = 0f;
        }

        public void UpdateHealthBar()
        {
            if (fillImage == null || playerStats == null) return;
            float ratio = (float)playerStats.CurrentHP / playerStats.MaxHP;
            fillImage.fillAmount = Mathf.Clamp01(ratio);
        }
    }
}
