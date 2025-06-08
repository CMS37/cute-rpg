using UnityEngine;
using System.Collections.Generic;

namespace Game.Utils
{
    public static class IconLoader
    {
        private static Dictionary<string, Sprite> cache;

        public static Sprite GetIcon(string iconName)
        {
            if (cache == null)
            {
                cache = new Dictionary<string, Sprite>();
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
                        if (!cache.ContainsKey(sp.name))
                            cache.Add(sp.name, sp);
                    }
                }
            }

            if (cache.TryGetValue(iconName, out var sprite))
                return sprite;

            Debug.LogWarning($"[IconLoader] '{iconName}' 아이콘을 찾을 수 없습니다.");
            return null;
        }
    }
}
