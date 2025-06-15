using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using Game.Data;
using Game.Utils;


namespace Game.Editor
{
    public static class ItemDataImporter
    {
        private const string ItemsFolder = "Assets/Resources/Items";
        private const string DatabasePath = "Assets/Resources/ItemDatabase.asset";

        [MenuItem("Tools/Import/Items From CSV & Update DB")]
        public static void ImportAndUpdateDatabase()
        {
            string csvPath = EditorUtility.OpenFilePanel("CSV 파일 선택", "", "csv");
            if (string.IsNullOrEmpty(csvPath))
                return;

            var lines = File.ReadAllLines(csvPath);
            if (lines.Length <= 1)
            {
                Debug.LogWarning("[ItemDataImporter] CSV 파일에 데이터가 없습니다.");
                return;
            }

            EnsureFolderExists(ItemsFolder);
            Debug.Log("[ItemDataImporter] CSV에서 아이템을 임포트합니다...");

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var cols = line.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (cols.Length < 8) continue;

                string id   = cols[0].Trim();
                string name = cols[1].Trim();
                string type = cols[2].Trim();
                string icon = cols[3].Trim();
                int atk     = int.Parse(cols[4]);
                int def     = int.Parse(cols[5]);
                int maxSt   = int.Parse(cols[6]);
                string desc = cols[7].Trim();

                string assetPath = Path.Combine(ItemsFolder, id + ".asset");
                var data = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath) ?? ScriptableObject.CreateInstance<ItemData>();

                data.Id           = id;
                data.Name         = name;
                data.Type         = (ItemType)Enum.Parse(typeof(ItemType), type);
                data.AttackPower  = atk;
                data.DefensePower = def;
                data.MaxStack     = maxSt;
                data.Description  = desc;
                data.Icon         = IconLoader.GetIcon(icon);

                if (!File.Exists(assetPath))
                    AssetDatabase.CreateAsset(data, assetPath);
                EditorUtility.SetDirty(data);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            UpdateDatabase();
            Debug.Log("[ItemDataImporter] 아이템 임포트 및 데이터베이스 업데이트 완료.");
        }

        private static void EnsureFolderExists(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                var parent = Path.GetDirectoryName(path);
                var name   = Path.GetFileName(path);
                AssetDatabase.CreateFolder(parent, name);
            }
        }

        private static void UpdateDatabase()
        {
            var db = AssetDatabase.LoadAssetAtPath<ItemDatabase>(DatabasePath) ?? ScriptableObject.CreateInstance<ItemDatabase>();
            var guids = AssetDatabase.FindAssets("t:ItemData", new[] { ItemsFolder });
            var items = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(item => item != null)
                .ToList();

            db.AllItems = items;

            if (!File.Exists(DatabasePath))
                AssetDatabase.CreateAsset(db, DatabasePath);
            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
