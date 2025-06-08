using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/Item Database", fileName = "ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        [Tooltip("List of all items in the project.")]
        [SerializeField] private List<ItemData> allItems = new List<ItemData>();

        private Dictionary<string, ItemData> lookup;

        private void OnEnable()
        {
            InitializeLookup();
        }

        private void InitializeLookup()
        {
            lookup = allItems?
                .Where(item => item != null && !string.IsNullOrWhiteSpace(item.Id))
                .ToDictionary(item => item.Id, item => item)
                ?? new Dictionary<string, ItemData>();
        }

        public ItemData Get(string id)
        {
            if (lookup == null)
                InitializeLookup();

            lookup.TryGetValue(id, out var item);
            return item;
        }
        public List<ItemData> AllItems { get => allItems; set => allItems = value; }
    }
}
