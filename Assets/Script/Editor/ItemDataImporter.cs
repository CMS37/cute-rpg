using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class ItemDataImporter
{
    const string CSV_PATH = "Assets/Data/items.csv";
    const string OUTPUT_FOLDER = "Assets/ScriptableObjects/Items";

    [MenuItem("Tools/Import/Items from CSV")]
    public static void ImportFromCSV()
    {
        if (!File.Exists(CSV_PATH))
        {
            Debug.LogError($"[{nameof(ItemDataImporter)}] CSV 파일을 찾을 수 없습니다: {CSV_PATH}");
            return;
        }

        if (!AssetDatabase.IsValidFolder(OUTPUT_FOLDER))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Items");

        var lines = File.ReadAllLines(CSV_PATH)
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .ToArray();
        if (lines.Length < 2)
        {
            Debug.LogWarning("CSV에 데이터가 없습니다.");
            return;
        }

        var header = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            var cols = lines[i].Split(',');
            if (cols.Length != header.Length) continue;

            string id            = cols[0];
            string name          = cols[1];
            string typeStr       = cols[2];
            string iconName      = cols[3];
            int    atk           = int.Parse(cols[4]);
            int    def           = int.Parse(cols[5]);
            int    maxStack      = int.Parse(cols[6]);
            string desc          = cols[7];

            string assetPath = $"{OUTPUT_FOLDER}/{id}_{name}.asset";

            var data = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath)
                      ?? ScriptableObject.CreateInstance<ItemData>();

            data.id           = id;
            data.itemName     = name;
            data.type         = System.Enum.TryParse<ItemType>(typeStr, out var t) ? t : ItemType.Material;
            data.attackPower  = atk;
            data.defensePower = def;
            data.maxStack     = maxStack;
            data.description  = desc;

            data.icon = IconLoader.GetIcon(iconName);

            if (AssetDatabase.Contains(data))
                EditorUtility.SetDirty(data);
            else
                AssetDatabase.CreateAsset(data, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[{nameof(ItemDataImporter)}] 아이템 임포트 완료: {lines.Length-1}개");
    }
}
