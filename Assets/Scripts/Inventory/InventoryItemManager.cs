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

    public void OnDrop(InventorySlot slot, PointerEventData eventData)
    {       

        if (eventData.pointerDrag == null)
        {
            return;
        }

        ChipUI chipUI = eventData.pointerDrag.GetComponent<ChipUI>();
        RectTransform chipRect = eventData.pointerDrag.GetComponent<RectTransform>();

        // Parent chip to item manager
        chipRect.transform.parent = transform;

        //Sets chip to slot using central position of chip
        //chipRect.anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;

        Vector2 firstChipCellPos = chipUI.GetFirstChipCellPosition(true);

        //X = floorToInt((world_red_dot_anchored_position.x / inventory_grid_width) * slots_in_row)

        //Y = floorToInt((world_red_dot_anchored_position.y / inventory_grid_height) * slots_in_column)

        float x = Mathf.FloorToInt((firstChipCellPos.x / 805f) * 5);
        float y = Mathf.FloorToInt(Mathf.Abs((firstChipCellPos.y / 805f) * 5));


        print("Grid x = " + x + " y = " + y);

    }

    public void OnEnter(InventorySlot slot, PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        ChipUI draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        draggedChip.DesiredParent = transform;
    }

    public void OnExit(InventorySlot slot, PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        ChipUI draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        draggedChip.DesiredParent = null;
    }
}
