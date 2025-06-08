using UnityEngine;

public enum ItemType { Material, Weapon, Consumable }

[CreateAssetMenu(menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
	public string id;
	public string itemName;
	public ItemType type;
	public Sprite icon;
	public int attackPower;
	public int defensePower;
	public int maxStack;
	[TextArea] public string description;
}
