using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private RectTransform rectTransform;

    GridLayoutGroup layoutGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponentInParent<GridLayoutGroup>();
    }

    public Vector2Int GetGridCoordinates()
    {
        Rect gridRect = layoutGroup.GetComponent<RectTransform>().rect;
        
        Vector2 slotSize = layoutGroup.cellSize;
        

        int x = Mathf.FloorToInt(rectTransform.localPosition.x / slotSize.x);
        int y = Mathf.FloorToInt(rectTransform.localPosition.y / slotSize.y);

        return new Vector2Int(x, y);
    }
}
