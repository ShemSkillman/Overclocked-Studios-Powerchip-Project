using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryNearbyItems : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //if (eventData.pointerDrag != null)
        //{
        //    eventData.pointerDrag.transform.SetParent(transform);
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        InventoryChip draggedChip = eventData.pointerDrag.GetComponent<InventoryChip>();

        draggedChip.DesiredParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        InventoryChip draggedChip = eventData.pointerDrag.GetComponent<InventoryChip>();

        draggedChip.DesiredParent = null;
    }
}
