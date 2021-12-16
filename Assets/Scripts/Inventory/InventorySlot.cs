using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InventorySlotEvent : UnityEvent<InventorySlot>
{
}

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;

    public delegate void OnButtonClickDelegate(InventorySlot slot);
    public OnButtonClickDelegate OnSlotDrop, OnSlotEnter, OnSlotExit;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnSlotDrop?.Invoke(this);

        //if (eventData.pointerDrag == null || IsOccupied())
        //{
        //    return;
        //}

        //ChipUI draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        //GetComponent<GridLayoutGroup>().cellSize = new Vector2(157 * draggedChip.itemData.chipLayoutMap.GetSize2D().x, 157 * draggedChip.itemData.chipLayoutMap.GetSize2D().y);
    }

    private bool IsOccupied()
    {
        return transform.childCount > 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnSlotEnter?.Invoke(this);

        //if (eventData.pointerDrag == null || IsOccupied())
        //{
        //    return;
        //}

        //ChipUI draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        //draggedChip.DesiredParent = transform;        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnSlotExit?.Invoke(this);

        //if (eventData.pointerDrag == null || IsOccupied())
        //{
        //    return;
        //}

        //ChipUI draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        //draggedChip.DesiredParent = null;
    }
}
