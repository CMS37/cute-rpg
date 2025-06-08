using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Core Managers")]
        public InputManager     inputManager;
        public InventoryManager inventoryManager;
        public UIManager        uiManager;
        // public QuestManager     questManager;
        // public MonsterManager   monsterManager;
        // public SaveLoadManager  saveLoadManager;

        [HideInInspector] public string nextSceneToLoad = "";
        [HideInInspector] public Vector2 nextSpawnPosition = Vector2.zero;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                SceneManager.sceneLoaded += OnSceneLoaded;

                if (inputManager == null)
                {
                    var imGO = new GameObject("InputManager");
                    imGO.transform.parent = this.transform;
                    inputManager = imGO.AddComponent<InputManager>();
                }

                if (inventoryManager == null)
                {
                    var invGO = new GameObject("InventoryManager");
                    invGO.transform.parent = this.transform;
                    inventoryManager = invGO.AddComponent<InventoryManager>();
                }
                inventoryManager.itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");                

                if (uiManager == null)
                {
                    var uiGO = new GameObject("UIManager");
                    uiGO.transform.parent = this.transform;
                    uiManager = uiGO.AddComponent<UIManager>();
                }


                // if (questManager == null)
                // {
                //     var qGO = new GameObject("QuestManager");
                //     qGO.transform.parent = this.transform;
                //     questManager = qGO.AddComponent<QuestManager>();
                // }

                // if (monsterManager == null)
                // {
                //     var mGO = new GameObject("MonsterManager");
                //     mGO.transform.parent = this.transform;
                //     monsterManager = mGO.AddComponent<MonsterManager>();
                // }

                // if (saveLoadManager == null)
                // {
                //     var sGO = new GameObject("SaveLoadManager");
                //     sGO.transform.parent = this.transform;
                //     saveLoadManager = sGO.AddComponent<SaveLoadManager>();
                // }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!string.IsNullOrEmpty(nextSceneToLoad) && scene.name == nextSceneToLoad)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector3 newPos = new Vector3(nextSpawnPosition.x, nextSpawnPosition.y, player.transform.position.z);
                    player.transform.position = newPos;
                }
                nextSceneToLoad = "";
                nextSpawnPosition = Vector2.zero;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
