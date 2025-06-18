using UnityEngine;
using UnityEngine.UI;
using Game.Managers;
using Game.Data;

namespace Game.UI
{
    /// <summary>
    /// 장착 슬롯 UI를 관리합니다.
    /// EquipPanelBG에 붙여서 장착 슬롯 상태를 표시합니다.
    /// </summary>
    public class EquipUI : MonoBehaviour
    {
        [Tooltip("장착된 무기 아이콘을 표시할 Image")]  
        [SerializeField] private Image weaponIcon;

        private int slotIndex;
        private EquipmentManager equipmentManager;

        private void Awake()
        {
            // 슬롯 인덱스 파싱
            var name = gameObject.name;
            if (name.Contains("_") && int.TryParse(name.Split('_')[1], out var idx))
                slotIndex = idx;
            else
                slotIndex = 0;

            // EquipmentManager 캐싱
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

        // 이벤트 핸들러: 해당 슬롯에 변화가 있을 때만 갱신
        private void OnEquipChanged(int changedSlot)
        {
            if (changedSlot == slotIndex)
                UpdateUI();
        }

        /// <summary>
        /// 현재 슬롯에 장착된 무기 아이콘으로 갱신
        /// </summary>
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
