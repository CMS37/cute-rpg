using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        [Header("Item Database")]
        public ItemDatabase itemDatabase;

        private Dictionary<string,int> items = new Dictionary<string,int>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                if (itemDatabase == null)
                {
                    itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
                    if (itemDatabase == null)
                        Debug.LogError("[InventoryManager] Failed to load ItemDatabase from Resources/ItemDatabase.asset");
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Add(string itemId, int qty = 1)
        {
            if (itemDatabase == null)
            {
                Debug.LogError("[InventoryManager] itemDatabase is null!");
                return;
            }

            if (itemDatabase.Get(itemId) == null)
            {
                Debug.LogWarning($"[Inventory] Unknown Item ID: {itemId}");
                return;
            }
            if (items.ContainsKey(itemId)) items[itemId] += qty;
            else items[itemId] = qty;

            UIManager.Instance?.RefreshInventoryUI();
        }

        public bool Use(string itemId)
        {
            if (!items.TryGetValue(itemId, out var count) || count <= 0)
                return false;

            items[itemId]--;
            if (items[itemId] == 0) items.Remove(itemId);

            UIManager.Instance?.RefreshInventoryUI();
            return true;
        }

        public List<(ItemData data, int count)> GetInventory()
        {
            var list = new List<(ItemData, int)>();
            foreach (var kv in items)
                list.Add((itemDatabase.Get(kv.Key), kv.Value));
            return list;
        }
    }
}
