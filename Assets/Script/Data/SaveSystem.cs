using UnityEngine;
using System.IO;
using System;

namespace Game.Data
{
    public static class SaveSystem
    {
        // 파일 경로 반환
        private static string GetPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }

        /// <summary>
        /// 데이터를 JSON으로 직렬화하여 파일에 저장합니다.
        /// </summary>
        public static void Save<T>(string fileName, T data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, prettyPrint: true);
                File.WriteAllText(GetPath(fileName), json);
#if UNITY_EDITOR
                Debug.Log($"[SaveSystem] Saved to {GetPath(fileName)}");
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Save failed: {e}");
            }
        }

        /// <summary>
        /// 파일에서 JSON을 읽어와 객체로 역직렬화합니다.
        /// 파일이 없거나 파싱 오류 시 null을 반환합니다.
        /// </summary>
        public static T Load<T>(string fileName) where T : class
        {
            string path = GetPath(fileName);
            if (!File.Exists(path))
            {
#if UNITY_EDITOR
                Debug.Log($"[SaveSystem] Load skipped, file not found: {path}");
#endif
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
