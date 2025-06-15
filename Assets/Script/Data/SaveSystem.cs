using UnityEngine;
using System.IO;
using System;

namespace Game.Data
{
    public static class SaveSystem
    {
        private static string GetPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }

        public static void Save<T>(string fileName, T data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, prettyPrint: true);
                File.WriteAllText(GetPath(fileName), json);
                Debug.Log($"[SaveSystem] Saved to {GetPath(fileName)}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Save failed: {e}");
            }
        }

        public static T Load<T>(string fileName) where T : class
        {
            string path = GetPath(fileName);
            if (!File.Exists(path))
            {
                Debug.Log($"[SaveSystem] Load skipped, file not found: {path}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Load failed: {e}");
                return null;
            }
        }
    }
}
