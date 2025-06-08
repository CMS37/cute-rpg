using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Core Managers")]
        [SerializeField] private InputManager inputManager;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private UIManager uiManager;
        // [SerializeField] private QuestManager questManager;
        // [SerializeField] private MonsterManager monsterManager;
        // [SerializeField] private SaveLoadManager saveLoadManager;

        public InputManager InputManager => inputManager;
        public InventoryManager InventoryManager => inventoryManager;
        public UIManager UIManager => uiManager;

        [Header("Scene Transition")]
        [Tooltip("다음 로드할 씬 이름")]
        [SerializeField] private string nextSceneToLoad = "";
        [Tooltip("씬 전환 후 플레이어 스폰 위치")]
        [SerializeField] private Vector2 nextSpawnPosition = Vector2.zero;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // 씬 로드 이벤트 등록
                SceneManager.sceneLoaded += OnSceneLoaded;

                // Core 매니저들 생성 및 자식으로 배치
                CreateManager(ref inputManager,  "InputManager");
                CreateManager(ref inventoryManager, "InventoryManager");
                CreateManager(ref uiManager,        "UIManager");
                //CreateManager(ref questManager,     "QuestManager");
                //CreateManager(ref monsterManager,   "MonsterManager");
                //CreateManager(ref saveLoadManager,  "SaveLoadManager");

                inventoryManager.SetItemDatabase(Resources.Load<Game.Data.ItemDatabase>("ItemDatabase"));
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!string.IsNullOrEmpty(nextSceneToLoad) && scene.name == nextSceneToLoad)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    var pos = player.transform.position;
                    player.transform.position = new Vector3(
                        nextSpawnPosition.x,
                        nextSpawnPosition.y,
                        pos.z);
                }
                nextSceneToLoad   = "";
                nextSpawnPosition = Vector2.zero;
            }
        }

        private void CreateManager<T>(ref T field, string gameObjectName) where T : MonoBehaviour
        {
            if (field == null)
            {
                var go = new GameObject(gameObjectName);
                go.transform.SetParent(transform, false);
                field = go.AddComponent<T>();
            }
        }

        public void SetNextScene(string sceneName, Vector2 spawnPos)
        {
            nextSceneToLoad   = sceneName;
            nextSpawnPosition = spawnPos;

            Game.System.SceneTransition.Instance.FadeToScene(sceneName);
        }
    }
}
