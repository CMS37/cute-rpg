[System.Serializable]
public class SaveData
{
    public PlayerData player = new PlayerData();
}

[System.Serializable]
public class PlayerData
{
    public int currentHP;
    public int maxHP;
    public int attack;
    public int defense;
    public float posX;
    public float posY;
}
