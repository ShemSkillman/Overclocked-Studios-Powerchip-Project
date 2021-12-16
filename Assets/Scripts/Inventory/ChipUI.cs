using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChipUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public string id;

    private Canvas canvas;

    public ItemScriptableObject itemData;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Image image;

    public Transform DesiredParent { get; set; }

    public Transform PreviousParent { get; set; }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();

        if (id == "")
        {
            id = System.Guid.NewGuid().ToString();
        }
    }

    private void Start()
    {
        image.preserveAspect = true;
        image.sprite = itemData.chipSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PreviousParent = transform.parent;

        canvasGroup.blocksRaycasts = false;

        transform.SetParent(canvas.transform);

        rectTransform.sizeDelta = new Vector2(161 * itemData.chipLayoutMap.GetSize2D().x, 161 * itemData.chipLayoutMap.GetSize2D().y);

        transform.position = Input.mousePosition;
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
