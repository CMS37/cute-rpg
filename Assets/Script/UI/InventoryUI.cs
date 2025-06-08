using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Game.Managers;

namespace Game.UI
{

    public class InventoryUI : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("인벤토리 전체 패널")]      [SerializeField] private GameObject panel;
        [Tooltip("슬롯 프리팹")]
        [SerializeField] private GameObject slotPrefab;
        [Tooltip("슬롯을 담을 컨테이너")]
        [SerializeField] private Transform bagSlotParent;

        [Header("Grid Settings")]
        [Tooltip("가로 슬롯 개수")]
        [SerializeField] private int bagColumns = 5;
        [Tooltip("세로 슬롯 개수")]
        [SerializeField] private int bagRows    = 5;

        private void Start()
        {
            panel.SetActive(false);
            GameManager.Instance.InputManager.OnInventoryToggle += ToggleInventory;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.InputManager.OnInventoryToggle -= ToggleInventory;
        }

        private void ToggleInventory()
        {
            panel.SetActive(!panel.activeSelf);
            if (panel.activeSelf)
                RefreshBag();
        }

        public void RefreshBag()
        {
            foreach (Transform child in bagSlotParent)
                Destroy(child.gameObject);

            var inv = InventoryManager.Instance.GetInventory();
            int capacity = bagColumns * bagRows;

            for (int i = 0; i < capacity; i++)
            {
                var slot = Instantiate(slotPrefab, bagSlotParent);
                var iconImage = slot.transform.Find("Icon").GetComponent<Image>();
                var countText = slot.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                var button    = slot.GetComponent<Button>();

                if (i < inv.Count)
                {
                    var (data, count) = inv[i];
                    iconImage.enabled = true;
                    iconImage.sprite  = data.Icon;
                    countText.text    = count.ToString();

                    string id = data.Id;
                    button.onClick.AddListener(() => InventoryManager.Instance.Use(id));
                }
                else
                {
                    iconImage.enabled = false;
                    countText.text    = string.Empty;
                    button.onClick.RemoveAllListeners();
                }
            }
        }
    }
}
