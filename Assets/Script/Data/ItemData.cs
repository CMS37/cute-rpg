using UnityEngine;

namespace Game.Data
{
    public enum ItemType
    {
        Material,
        Weapon,
        Consumable
    }

    [CreateAssetMenu(menuName = "Game/Item Data", fileName = "New ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Identification")]
        [SerializeField] private string id;
        [SerializeField] private string itemName;

        [Header("Type & Icon")]
        [SerializeField] private ItemType type;
        [SerializeField] private Sprite icon;

        [Header("Stats")]
        [SerializeField] private int attackPower;
        [SerializeField] private int defensePower;
        [SerializeField] private int maxStack = 1;

        [Header("Description")]
        [TextArea]
        [SerializeField] private string description;

        public string Id { get => id; set => id = value; }
        public string Name { get => itemName; set => itemName = value; }
        public ItemType Type { get => type; set => type = value; }
        public Sprite Icon { get => icon; set => icon = value; }
        public int AttackPower { get => attackPower; set => attackPower = value; }
        public int DefensePower { get => defensePower; set => defensePower = value; }
        public int MaxStack { get => maxStack; set => maxStack = value; }
        public string Description { get => description; set => description = value; }
    }
}