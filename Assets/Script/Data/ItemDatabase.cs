using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Game/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> allItems;

    // 런타임용 캐시
    private Dictionary<string, ItemData> lookup;

    private void OnEnable()
    {
        lookup = allItems
            .Where(i => !string.IsNullOrEmpty(i.id))
            .ToDictionary(i => i.id, i => i);
    }

    public ItemData Get(string id)
    {
        lookup.TryGetValue(id, out var data);
        return data;
    }
}
