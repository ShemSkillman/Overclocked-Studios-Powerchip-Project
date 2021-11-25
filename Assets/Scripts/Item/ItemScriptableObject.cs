using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    private string id;

    public PickUp pickUp;

    public InventoryChip inventoryChip;

    public string ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }
}