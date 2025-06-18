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

        private void OnEnable()
        {
            inventoryManager.OnInventoryChanged += RefreshInventoryUI;
        }

        private void OnDisable()
        {
            inventoryManager.OnInventoryChanged -= RefreshInventoryUI;
        }

        public void OpenMenu()
        {
            inventoryPanel.SetActive(true);
            RefreshInventoryUI();
        }

        public void CloseMenu()
        {
            inventoryPanel.SetActive(false);
        }

        public void OnItemSelected(int index)
        {
            selectedIndex = index;
            // 선택된 아이템에 따라 버튼 활성화 등 처리
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
                equipmentManager.EquipToSlot(selectedIndex, 0); // 0번 슬롯 예시
            }
        }

        private void RefreshInventoryUI()
        {
            // 인벤토리 리스트 UI 갱신 로직 구현
        }
    }
}