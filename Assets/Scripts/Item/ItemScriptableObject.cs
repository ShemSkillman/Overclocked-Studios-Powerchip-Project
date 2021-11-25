using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;

    public PickUp pickUp;
    public InventoryChip inventoryChip;
}