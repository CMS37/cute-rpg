using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public PlayerData player;
    public List<InventoryItemData> inventory = new List<InventoryItemData>();
    // 필요시 퀘스트, 장비, 기타 데이터 추가

    // 예시: 저장 버전
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
    // 필요시 레벨, 경험치 등 추가
}

[Serializable]
public class InventoryItemData
{
    public string itemId;
    public int count;
    // 필요시 장착 여부, 강화 수치 등 추가
}
