using System.Collections.Generic;
using UnityEngine;
using Game.Data;

namespace Game.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [Header("Item Database")]
        [SerializeField] private ItemDatabase itemDatabase;

        public void SetItemDatabase(Game.Data.ItemDatabase db)
        {
            itemDatabase = db;
        }

        private readonly Dictionary<string, int> items = new Dictionary<string, int>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                if (itemDatabase == null)
                {
                    itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
                    if (itemDatabase == null)
                        Debug.LogError("[InventoryManager] ItemDatabase를 로드하지 못했습니다.");
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Add(string itemId, int quantity = 1)
        {
            if (itemDatabase == null) return;
            var data = itemDatabase.Get(itemId);
            if (data == null)
            {
                Debug.LogWarning($"[InventoryManager] 알 수 없는 아이템 ID: {itemId}");
                return;
            }

            if (items.ContainsKey(itemId))
                items[itemId] += quantity;
            else
                items[itemId] = quantity;

            UIManager.Instance?.RefreshInventoryUI();
        }

        public bool Use(string itemId)
        {
            if (!items.TryGetValue(itemId, out var count) || count <= 0)
                return false;

            items[itemId]--;
            if (items[itemId] == 0)
                items.Remove(itemId);

            UIManager.Instance?.RefreshInventoryUI();
            return true;
        }

        public List<(ItemData data, int count)> GetInventory()
        {
            var list = new List<(ItemData, int)>();
            foreach (var kv in items)
            {
                var data = itemDatabase.Get(kv.Key);
                list.Add((data, kv.Value));
            }
            return list;
        }
    }
}
