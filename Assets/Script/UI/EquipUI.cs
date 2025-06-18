using UnityEngine;
using UnityEngine.UI;
using Game.Managers;
using Game.Data;

namespace Game.UI
{
    public class EquipUI : MonoBehaviour
    {
        [Tooltip("장착된 무기 아이콘을 표시할 Image")]  
        [SerializeField] private Image weaponIcon;

        private int slotIndex;
        private EquipmentManager equipmentManager;

        private void Awake()
        {
            var name = gameObject.name;
            if (name.Contains("_") && int.TryParse(name.Split('_')[1], out var idx))
                slotIndex = idx;
            else
                slotIndex = 0;

            equipmentManager = GameManager.Instance.EquipmentManager;
        }

        private void OnEnable()
        {
            UpdateUI();
            if (equipmentManager != null)
                equipmentManager.onEquipChanged += OnEquipChanged;
        }

        private void OnDisable()
        {
            if (equipmentManager != null)
                equipmentManager.onEquipChanged -= OnEquipChanged;
        }

        private void OnEquipChanged(int changedSlot)
        {
            if (changedSlot == slotIndex)
                UpdateUI();
        }

        public void UpdateUI()
        {
            var weapon = equipmentManager.GetEquippedInSlot(slotIndex);
            if (weapon != null)
            {
                weaponIcon.sprite = weapon.Icon;
                weaponIcon.enabled = true;
            }
            else
            {
                weaponIcon.enabled = false;
            }
        }
    }
}
