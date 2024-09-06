using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Weapon,
    Default

}

public abstract class ItemSO : ScriptableObject
{
    public int Id;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public string interactText;

}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;

    public Item(ItemSO item)
    {
        Name = item.name;
        Id = item.Id;
    }
}
