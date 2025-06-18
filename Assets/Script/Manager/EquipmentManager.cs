using System;
using UnityEngine;
using Game.Data;
using Game.Player;

namespace Game.Managers
{
    public class EquipmentManager : MonoBehaviour
    {
        [Header("Equip Slot Count")]
        [Tooltip("Number of weapon slots available for equipping")]  
        [SerializeField] private int slotCount = 7;

        private CharacterStats stats;
        private ItemData[] equippedWeapons;
        private int equippedInventoryIndex = -1;
        public int EquippedInventoryIndex => equippedInventoryIndex;
        public int SlotCount => equippedWeapons.Length;

        public event Action<int> onEquipChanged;

        private InventoryManager inventoryManager;

        private void Awake()
        {
            equippedWeapons = new ItemData[slotCount];
            inventoryManager = GameManager.Instance.InventoryManager;
        }

        private void EnsureStats()
        {
            if (stats == null && PlayerController.Instance != null)
                stats = PlayerController.Instance.GetComponent<CharacterStats>();
        }

        public void EquipToSlot(int inventoryIndex, int equipSlot)
        {
            if (equipSlot != 0) return;

            var inv = inventoryManager.GetInventory();
            if (inventoryIndex < 0 || inventoryIndex >= inv.Count) return;
            var (data, _) = inv[inventoryIndex];
            if (data.Type != ItemType.Weapon) return;

            if (equippedInventoryIndex == inventoryIndex)
            {
                UnequipSlot(0);
                return;
            }

            equippedWeapons[equipSlot]         = data;
            equippedInventoryIndex             = inventoryIndex;
            ApplyStats();
            onEquipChanged?.Invoke(equipSlot);
            Debug.Log($"Equipped {data.Name} (invIndex={inventoryIndex}) to slot {equipSlot}");
        }

        public void UnequipSlot(int equipSlot)
        {
            if (equipSlot != 0) return;

            equippedWeapons[equipSlot]        = null;
            equippedInventoryIndex            = -1;
            ApplyStats();
            onEquipChanged?.Invoke(equipSlot);
            Debug.Log($"Unequipped slot {equipSlot}");
        }

        public ItemData GetEquippedInSlot(int equipSlot)
        {
            if (equipSlot < 0 || equipSlot >= slotCount) return null;
            return equippedWeapons[equipSlot];
        }

        public bool HasEquippedWeapon(ItemData weapon)
        {
            if (weapon == null) return false;
            foreach (var w in equippedWeapons)
            {
                if (w != null && w.Id == weapon.Id)
                    return true;
            }
            return false;
        }

        private void ApplyStats()
        {
            EnsureStats();
            if (stats == null) return;

            int totalBonus = 0;
            foreach (var w in equippedWeapons)
            {
                if (w != null)
                    totalBonus += w.AttackPower;
            }
            stats.attack.bonusValue = totalBonus;
        }
    }
}
