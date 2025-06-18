using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using System;

namespace Game.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        [Header("Item Database")]
        [SerializeField] private ItemDatabase itemDatabase;

        private readonly List<(ItemData data, int count)> items = new List<(ItemData, int)>();

        public event Action OnInventoryChanged;

        public void SetItemDatabase(ItemDatabase db)
        {
            itemDatabase = db;
        }

        private void Awake()
        {
            if (itemDatabase == null)
            {
                itemDatabase = Resources.Load<ItemDatabase>("ItemDatabase");
                if (itemDatabase == null)
                    Debug.LogError("[InventoryManager] ItemDatabase를 로드하지 못했습니다.");
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
            if (data.Type == ItemType.Weapon)
            {
                for (int i = 0; i < quantity; i++)
                    items.Add((data, 1));
            }
            else
            {
                int idx = items.FindIndex(x => x.data.Id == itemId);
                if (idx >= 0)
                {
                    int newCount = Mathf.Clamp(items[idx].count + quantity, 1, data.MaxStack);
                    items[idx] = (data, newCount);
                }
                else
                {
                    items.Add((data, Mathf.Clamp(quantity, 1, data.MaxStack)));
                }
            }
            OnInventoryChanged?.Invoke();
        }

        public bool Use(string itemId)
        {
            int idx = items.FindIndex(x => x.data.Id == itemId && x.data.Type != ItemType.Weapon);
            if (idx < 0) return false;

            var entry = items[idx];
            int newCount = entry.count - 1;
            if (newCount <= 0)
                items.RemoveAt(idx);
            else
                items[idx] = (entry.data, newCount);

            OnInventoryChanged?.Invoke();
            return true;
        }

        public IReadOnlyList<(ItemData data, int count)> GetInventory()
        {
            return items.AsReadOnly();
        }
    }
}
