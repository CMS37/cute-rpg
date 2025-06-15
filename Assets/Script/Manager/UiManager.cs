using UnityEngine;
using UnityEngine.UI;
using Game.Managers;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private GameObject pauseMenuCanvas;
        private GameObject inventoryMenuCanvas;
        private bool isPaused;
        private bool isInventoryOpen;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            var root = GameManager.Instance.transform.Find("CanvasContainer");
            if (root == null)
            {
                Debug.LogError("UIManager: CanvasContainer를 찾을 수 없습니다.");
                return;
            }

            pauseMenuCanvas     = root.Find("PauseMenuCanvas")?.gameObject;
            inventoryMenuCanvas = root.Find("InventoryMenuCanvas")?.gameObject;


            if (pauseMenuCanvas    != null)
            {
                pauseMenuCanvas.SetActive(false);

                var saveBtn = pauseMenuCanvas.transform.Find("Panel/SaveButton")?.GetComponent<Button>();
                if (saveBtn != null)
                    saveBtn.onClick.AddListener(SaveGame);
                else
                    Debug.LogWarning("UIManager: PauseMenuCanvas 내 SaveButton을 찾을 수 없습니다.");

                var quitBtn = pauseMenuCanvas.transform.Find("Panel/QuitButton")?.GetComponent<Button>();
                if (quitBtn != null)
                    quitBtn.onClick.AddListener(QuitGame);
                else
                    Debug.LogWarning("UIManager: PauseMenuCanvas 내 QuitButton을 찾을 수 없습니다.");
            }
            if (inventoryMenuCanvas!= null) inventoryMenuCanvas.SetActive(false);

            Time.timeScale = 1f;
        }

        public void TogglePauseMenu()
        {
            if (pauseMenuCanvas == null) return;
            isPaused = !isPaused;
            pauseMenuCanvas.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
        }

        public void ToggleInventoryMenu()
        {
            if (inventoryMenuCanvas == null) return;
            isInventoryOpen = !isInventoryOpen;
            inventoryMenuCanvas.SetActive(isInventoryOpen);
            if (isInventoryOpen)
                RefreshInventoryUI();
        }

        public void RefreshInventoryUI()
        {
            var invUI = FindObjectOfType<Game.UI.InventoryUI>();
            if (invUI != null)
                invUI.RefreshBag();
        }

        public void SaveGame()
        {
            Debug.Log("Call SaveGame");
            GameDataManager.Instance?.ManualSave();
        }

        public void QuitGame()
        {
            Debug.Log("Call EXIT GAME");
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}