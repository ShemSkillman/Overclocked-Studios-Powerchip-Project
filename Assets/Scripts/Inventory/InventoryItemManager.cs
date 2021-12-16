using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemManager : MonoBehaviour
{
    [SerializeField] Transform inventoryGrid;

    private void OnEnable()
    {
        for (int i = 0; i < inventoryGrid.childCount; i++)
        {
            InventorySlot slot = inventoryGrid.GetChild(i).gameObject.GetComponent<InventorySlot>();

            slot.OnSlotDrop += OnDrop;
            slot.OnSlotEnter += OnEnter;
            slot.OnSlotExit += OnExit;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < inventoryGrid.childCount; i++)
        {
            InventorySlot slot = inventoryGrid.GetChild(i).gameObject.GetComponent<InventorySlot>();

            slot.OnSlotDrop -= OnDrop;
            slot.OnSlotEnter -= OnEnter;
            slot.OnSlotExit -= OnExit;
        }
    }

    public void OnDrop(InventorySlot slot)
    {
        print(slot.GetGridCoordinates());

        //print("drop for slot " + slot.transform.position);
    }

    public void OnEnter(InventorySlot slot)
    {
        //print("entered slot " + slot.transform.position);
    }

    public void OnExit(InventorySlot slot)
    {
        //print("exited slot " + slot.transform.position);
    }
}
