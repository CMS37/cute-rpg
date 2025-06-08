using UnityEngine;
using System.Collections.Generic;

public static class IconLoader
{
    private static Dictionary<string, Sprite> _cache;

    public static Sprite GetIcon(string iconName)
    {
        if (_cache == null)
        {
            _cache = new Dictionary<string, Sprite>();
            // PNG 파일 이름들 (확장자 제외)
            string[] sheets = new[] {
                "Icons/Food_Icons_NO_Outline",
                "Icons/Other_Icons_NO_Outline",
                "Icons/Resources_Icons_NO_Outline",
                "Icons/Tool_Icons_NO_Outline"
            };
            foreach (var sheet in sheets)
            {
                var sprites = Resources.LoadAll<Sprite>(sheet);
                foreach (var sp in sprites)
                {
                    if (!_cache.ContainsKey(sp.name))
                        _cache.Add(sp.name, sp);
                }
            }
        }

        if (_cache.TryGetValue(iconName, out var icon))
            return icon;

        Debug.LogWarning($"IconLoader: '{iconName}' 아이콘을 찾을 수 없습니다.");
        return null;
    }
}
