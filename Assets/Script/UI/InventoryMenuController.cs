using UnityEngine;
using UnityEngine.UI;
using Game.Managers;
using Game.Data;

namespace Game.UI
{
    public class InventoryMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private Button useButton;
        [SerializeField] private Button equipButton;

        private InventoryManager inventoryManager;
        private EquipmentManager equipmentManager;
        private UIManager uiManager;

        private int selectedIndex = -1;

        private void Awake()
        {
            inventoryManager = GameManager.Instance.InventoryManager;
            equipmentManager = GameManager.Instance.EquipmentManager;
            uiManager        = GameManager.Instance.UIManager;

            if (useButton != null)
                useButton.onClick.AddListener(OnUseClicked);
            if (equipButton != null)
                equipButton.onClick.AddListener(OnEquipClicked);
        }

        public void CloseMenu()
        {
            inventoryPanel.SetActive(false);
        }

        public void OnItemSelected(int index)
        {
            selectedIndex = index;
        }

        private void OnUseClicked()
        {
            if (selectedIndex < 0) return;
            var item = inventoryManager.GetInventory()[selectedIndex].data;
            if (item.Type == ItemType.Consumable)
            {
                inventoryManager.Use(item.Id);
            }
        }

        private void OnEquipClicked()
        {
            if (selectedIndex < 0) return;
            var item = inventoryManager.GetInventory()[selectedIndex].data;
            if (item.Type == ItemType.Weapon)
            {
                equipmentManager.EquipToSlot(selectedIndex, 0);
            }
        }
    }
}