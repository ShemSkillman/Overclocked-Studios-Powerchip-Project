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

        print(slot.GetGridCoordinates());

        ChipUI chipUI = eventData.pointerDrag.GetComponent<ChipUI>();
        RectTransform chipRect = eventData.pointerDrag.GetComponent<RectTransform>();

        chipRect.transform.parent = transform;

        chipRect.anchoredPosition = slot.GetComponent<RectTransform>().anchoredPosition;

        //float offsetX = (chipRect.rect.width % 161) / 2;
   
        //float offsetY = (chipRect.rect.height % 161) / 2;

        //chipRect.anchoredPosition = new Vector2(chipRect.anchoredPosition.x + offsetX, chipRect.anchoredPosition.y + offsetY);

        Vector2 chipSize = chipUI.itemData.chipLayoutMap.GetSize2D();
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
