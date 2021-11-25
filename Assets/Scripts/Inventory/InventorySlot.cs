using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //if (eventData.pointerDrag != null && !IsOccupied())
        //{
        //    eventData.pointerDrag.transform.SetParent(transform);
        //    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(rectTransform.rect.width / 2, -rectTransform.rect.height / 2, 0);
        //}
    }

    private bool IsOccupied()
    {
        return transform.childCount > 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || IsOccupied())
        {
            return;
        }

        InventoryChip draggedChip = eventData.pointerDrag.GetComponent<InventoryChip>();

        draggedChip.DesiredParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || IsOccupied())
        {
            return;
        }

        InventoryChip draggedChip = eventData.pointerDrag.GetComponent<InventoryChip>();

        draggedChip.DesiredParent = null;
    }
}
