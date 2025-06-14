using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Game.Managers;
using Game.Data;

namespace Game.UI
{
    /// <summary>
    /// Inventory UI: draws bag slots and handles equip/unequip for a single weapon slot.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("UI References")]        
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform  bagSlotParent;

        private int bagColumns = 5;
        private int bagRows    = 5;

        /// <summary>
        /// Refresh the bag display.
        /// </summary>
        public void RefreshBag()
        {
            if (panel != null && !panel.activeSelf)
                return;

            // Clear old slots
            foreach (Transform child in bagSlotParent)
                Destroy(child.gameObject);

            var invList = InventoryManager.Instance.GetInventory();
            var equipMgr = EquipmentManager.Instance;
            int capacity = bagColumns * bagRows;

            for (int i = 0; i < capacity; i++)
            {
                var slot      = Instantiate(slotPrefab, bagSlotParent);
                var iconImage = slot.transform.Find("Icon").GetComponent<Image>();
                var countText = slot.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                var button    = slot.GetComponent<Button>();
                button.onClick.RemoveAllListeners();

                if (i < invList.Count)
                {
                    var (data, count) = invList[i];
                    iconImage.enabled = true;
                    iconImage.sprite  = data.Icon;

                    if (data.Type == ItemType.Weapon)
                    {
                        bool isEquipped = (equipMgr.EquippedInventoryIndex == i);
                        countText.text  = isEquipped ? "E" : string.Empty;

                        int invIndex = i;
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() =>
                        {
                            if (equipMgr.EquippedInventoryIndex == invIndex)
                                equipMgr.UnequipSlot(0);
                            else
                                equipMgr.EquipToSlot(invIndex, 0);
                            RefreshBag();
                        });
                    }
                    else
                    {
                        countText.text = count.ToString();
                        int invIndex = i;
                        button.onClick.AddListener(() =>
                        {
                            InventoryManager.Instance.Use(data.Id);
                            RefreshBag();
                        });
                    }
                }
                else
                {
                    iconImage.enabled = false;
                    countText.text    = string.Empty;
                }
            }
        }
    }
}
