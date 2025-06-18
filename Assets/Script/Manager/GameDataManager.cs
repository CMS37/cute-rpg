using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Player;
using Game.Data;
using System;

namespace Game.Managers
{
    public class GameDataManager : MonoBehaviour
    {
        private const string SAVE_FILE = "save.json";
        public SaveData Data { get; private set; }

        private bool hasRestoredInitialPosition = false;

        public event Action OnGameSaved;

        private void Awake()
        {
            Data = SaveSystem.Load<SaveData>(SAVE_FILE) ?? CreateNewData();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void Initialize()
        {
            hasRestoredInitialPosition = false;
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            hasRestoredInitialPosition = true;
        }

        private SaveData CreateNewData()
        {
            return new SaveData
            {
                player = new PlayerData
                {
                    maxHP     = 100,
                    currentHP = 100,
                    attack    = 5,
                    defense   = 0,
                    posX      = 0f,
                    posY      = 0f
                }
            };
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var pc = PlayerController.Instance;
            if (pc == null) return;

            var stats = pc.GetComponent<CharacterStats>();

            stats.SetMaxHP    (Data.player.maxHP);
            stats.SetCurrentHP(Data.player.currentHP);
            stats.SetAttack   (Data.player.attack);
            stats.SetDefense  (Data.player.defense);

            if (!hasRestoredInitialPosition)
            {
                pc.transform.position = new Vector2(Data.player.posX, Data.player.posY);
            }
        }

        private void SavePlayerStats()
        {
            var pc = PlayerController.Instance;
            if (pc == null) return;

            var stats = pc.GetComponent<CharacterStats>();
            Data.player.currentHP = stats.CurrentHP;
            Data.player.maxHP     = stats.MaxHP;
            Data.player.attack    = stats.CurrentAttack;
            Data.player.defense   = stats.CurrentDefense;

            var pos = pc.transform.position;
            Data.player.posX = pos.x;
            Data.player.posY = pos.y;

            SaveSystem.Save(SAVE_FILE, Data);
        }

        public void UpdateRuntimeData()
        {  
            var pc = PlayerController.Instance;
            if (pc == null) return;

            var stats = pc.GetComponent<CharacterStats>();
            Data.player.currentHP = stats.CurrentHP;
            Data.player.maxHP     = stats.MaxHP;
            var pos = pc.transform.position;
            Data.player.posX = pos.x;
            Data.player.posY = pos.y;
        }

        public void ManualSave()
        {
            SavePlayerStats();
            Debug.Log("[GameDataManager] Game saved.");
            OnGameSaved?.Invoke();
        }
    }
}