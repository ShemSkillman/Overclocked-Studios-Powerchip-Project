using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChipUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] Image debugSquarePrefab;

    public string id;

    private Canvas canvas;

    public ItemScriptableObject itemData;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Image image;

    public Transform DesiredParent { get; set; }

    public Transform PreviousParent { get; set; }

    Image debugSquare;

    public Outline targetOutline;

    public delegate void OnStartDrag(ChipUI chipUI);
    public OnStartDrag onStartDrag;

    public static float chipCellSize;

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

        debugSquare = Instantiate(debugSquarePrefab, Vector3.zero, Quaternion.identity, transform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        targetOutline.enabled = false;

        PreviousParent = transform.parent;

        canvasGroup.blocksRaycasts = false;

        transform.SetParent(canvas.transform);

        rectTransform.sizeDelta = new Vector2(chipCellSize * itemData.chipLayoutMap.GetSize2D().x, chipCellSize * itemData.chipLayoutMap.GetSize2D().y);

        transform.position = Input.mousePosition;

        debugSquare.enabled = true;

        onStartDrag?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Nearby Items being touched");

        targetOutline.enabled = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        debugSquare.rectTransform.anchoredPosition = GetLocalChipCellPosition(0,0);
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

        debugSquare.enabled = false;
    }

    public Vector2 GetLocalChipCellPosition(int x, int y)
    {
        Vector2 pivotCenter = new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2);
            
        Vector2 cellCentrePos = -pivotCenter + new Vector2((x * chipCellSize) + (chipCellSize / 2.0f), (y * chipCellSize) + (chipCellSize / 2.0f));

        return cellCentrePos;
    }

    public Vector2 GetWorldChipCellPosition(int x, int y)
    {
        return rectTransform.anchoredPosition + GetLocalChipCellPosition(x, y);
    }

    public Vector2 GetFirstChipCellPosition(bool isWorldPos)
    {
        bool[,] chipMap = itemData.chipLayoutMap.GetBoolean2DArray();

        for (int y = 0; y < chipMap.GetLength(1); y++)
        {
            for (int x = 0; x < chipMap.GetLength(0); x++)
            {
                if (chipMap[x, y] == true)
                {
                    if (isWorldPos)
                    {
                        return GetWorldChipCellPosition(x, y);
                    }
                    else
                    {
                        return GetLocalChipCellPosition(x, y);
                    }                    
                }
            }
        }

        return Vector2.zero;
    }
}
