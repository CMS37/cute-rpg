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
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform  bagSlotParent;

        private int bagColumns = 5;
        private int bagRows    = 5;

        public void RefreshBag()
        {
            if (panel != null && !panel.activeSelf)
                return;

            foreach (Transform child in bagSlotParent)
                Destroy(child.gameObject);

            var inv = InventoryManager.Instance.GetInventory();
            int capacity = bagColumns * bagRows;

            for (int i = 0; i < capacity; i++)
            {
                var slot      = Instantiate(slotPrefab, bagSlotParent);
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
