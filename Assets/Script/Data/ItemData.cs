using UnityEngine;

namespace Game.Data
{
    public enum ItemType
    {
        Material,
        Weapon,
        Consumable
    }

    public enum StatType
    {
        HP,
        Attack,
        Defense
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

        public string Id
        {
            get => id;
    #if UNITY_EDITOR
            set => id = value;
    #endif
        }
        public string Name
        {
            get => itemName;
    #if UNITY_EDITOR
            set => itemName = value;
    #endif
        }
        public ItemType Type
        {
            get => type;
    #if UNITY_EDITOR
            set => type = value;
    #endif
        }
        public Sprite Icon
        {
            get => icon;
    #if UNITY_EDITOR
            set => icon = value;
    #endif
        }
        public int AttackPower
        {
            get => attackPower;
    #if UNITY_EDITOR
            set => attackPower = value;
    #endif
        }
        public int DefensePower
        {
            get => defensePower;
    #if UNITY_EDITOR
            set => defensePower = value;
    #endif
        }
        public int MaxStack
        {
            get => maxStack;
    #if UNITY_EDITOR
            set => maxStack = value;
    #endif
        }
        public string Description
        {
            get => description;
    #if UNITY_EDITOR
            set => description = value;
    #endif
        }
    }
}