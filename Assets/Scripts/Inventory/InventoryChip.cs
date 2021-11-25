using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryChip : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public string id;

    [SerializeField]
    private Image image;

    private Canvas canvas;

    public ItemScriptableObject itemData;

    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    public Transform DesiredParent { get; set; }

    public Transform PreviousParent { get; set; }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (id == "")
        {
            id = System.Guid.NewGuid().ToString();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PreviousParent = transform.parent;

        canvasGroup.blocksRaycasts = false;

        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (DesiredParent != null)
        {
            transform.SetParent(DesiredParent);
        }
        else if (PreviousParent != transform.parent)
        {
            transform.SetParent(PreviousParent);
        }

        DesiredParent = null;

        canvasGroup.blocksRaycasts = true;
    }
}
