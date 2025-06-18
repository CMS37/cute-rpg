using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public PlayerData player;
    public List<InventoryItemData> inventory = new List<InventoryItemData>();
    public int version = 1;
}

[Serializable]
public class PlayerData
{
    public int maxHP;
    public int currentHP;
    public int attack;
    public int defense;
    public float posX;
    public float posY;
}

[Serializable]
public class InventoryItemData
{
    public string itemId;
    public int count;
}
