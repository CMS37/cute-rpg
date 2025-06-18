using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Game.Managers;
using Game.Data;

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

        private InventoryManager inventoryManager;
        private EquipmentManager equipmentManager;

        private void Awake()
        {
            inventoryManager = GameManager.Instance.InventoryManager;
            equipmentManager = GameManager.Instance.EquipmentManager;
        }

        private void OnEnable()
        {
            if (inventoryManager != null)
                inventoryManager.OnInventoryChanged += RefreshBag;
            if (equipmentManager != null)
                equipmentManager.onEquipChanged += _ => RefreshBag();

            RefreshBag();
        }

        private void OnDisable()
        {
            if (inventoryManager != null)
                inventoryManager.OnInventoryChanged -= RefreshBag;
            if (equipmentManager != null)
                equipmentManager.onEquipChanged -= _ => RefreshBag();
        }

        public void RefreshBag()
        {
            if (panel != null && !panel.activeSelf)
                return;

            foreach (Transform child in bagSlotParent)
                Destroy(child.gameObject);

            var invList = inventoryManager.GetInventory();
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
                        bool isEquipped = (equipmentManager.EquippedInventoryIndex == i);
                        countText.text  = isEquipped ? "E" : string.Empty;

                        int invIndex = i;
                        button.onClick.AddListener(() =>
                        {
                            if (equipmentManager.EquippedInventoryIndex == invIndex)
                                equipmentManager.UnequipSlot(0);
                            else
                                equipmentManager.EquipToSlot(invIndex, 0);
                        });
                    }
                    else
                    {
                        countText.text = count.ToString();
                        int invIndex = i;
                        button.onClick.AddListener(() =>
                        {
                            inventoryManager.Use(data.Id);
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
