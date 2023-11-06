using MyFolder.Script.InventoryScript;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipments", menuName = "Items/Equipments", order = 1)]
public class Items : ScriptableObject
{
    public ItemType itemType;
    public CharacterType allowedCharacterType;
    
    public string ItemName;
    public Sprite ItemIcon;
    public int quantity = 1;
    public int barrier;
    public Attribute attribute = new();
}