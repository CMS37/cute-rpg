using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Data;
using Game.Player;
using Game.System;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Core Managers")]
        [SerializeField] private InputManager       inputManager;
        [SerializeField] private InventoryManager   inventoryManager;
        [SerializeField] private UIManager          uiManager;
        [SerializeField] private GameDataManager    gameDataManager;
        [SerializeField] private EquipmentManager   equipmentManager;

        // [SerializeField] private QuestManager       questManager;
        // [SerializeField] private MonsterManager     monsterManager;

        public InputManager     InputManager     { get; private set; }
        public InventoryManager InventoryManager { get; private set; }
        public UIManager        UIManager        { get; private set; }
        public GameDataManager  GameDataManager  { get; private set; }
        public EquipmentManager EquipmentManager { get; private set; }


        private string  nextSceneToLoad   = "";
        private Vector2 nextSpawnPosition = Vector2.zero;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                SceneManager.sceneLoaded += OnSceneLoaded;

                CreateManager(ref inputManager,       "InputManager");
                CreateManager(ref inventoryManager,   "InventoryManager");
                CreateManager(ref uiManager,          "UIManager");
                CreateManager(ref gameDataManager,    "GameDataManager");
                CreateManager(ref equipmentManager,   "EquipmentManager");
                //CreateManager(ref questManager,      "QuestManager");
                //CreateManager(ref monsterManager,    "MonsterManager");

                InputManager     = inputManager;
                InventoryManager = inventoryManager;
                UIManager        = uiManager;
                GameDataManager  = gameDataManager;
                EquipmentManager = equipmentManager;

                GameDataManager.Initialize();

                var db = Resources.Load<ItemDatabase>("ItemDatabase");
                if (db != null)
                    inventoryManager.SetItemDatabase(db);
                else
                    Debug.LogWarning("[GameManager] ItemDatabase 로드 실패");
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
                if (PlayerController.Instance != null)
                {
                    var player = PlayerController.Instance.transform;
                    player.position = new Vector3(
                        nextSpawnPosition.x,
                        nextSpawnPosition.y,
                        player.position.z
                    );
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
            GameDataManager.Instance?.UpdateRuntimeData();

            nextSceneToLoad   = sceneName;
            nextSpawnPosition = spawnPos;
            SceneTransition.Instance.FadeToScene(sceneName);
        }
    }
}
