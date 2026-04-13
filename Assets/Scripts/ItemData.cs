using UnityEngine;

public enum ItemType
{
    Consumable,
    Material,
    Weapon,
    Armor,
    Quest,
    Misc
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea(2, 5)] public string description;
    public Sprite icon;
    public ItemType itemType = ItemType.Misc;
    public bool stackable = true;
    public int maxStack = 99;
}
