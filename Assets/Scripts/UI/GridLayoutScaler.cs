using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutScaler : MonoBehaviour
{
    private GridLayoutGroup gridLayout;
    RectTransform rectTransform;

    [SerializeField] RectTransform itemInventoryManagerRect;

    private void Awake()
    {
        gridLayout = GetComponentInChildren<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        float cellWidth = rectTransform.rect.width / gridLayout.constraintCount;
        float cellHeight = rectTransform.rect.height / gridLayout.constraintCount;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        float exactGridSize = cellSize * gridLayout.constraintCount;

        float offsetRight = exactGridSize / rectTransform.rect.width;
        float offsetBottom = exactGridSize / rectTransform.rect.height;

        itemInventoryManagerRect.anchorMax = new Vector2(offsetRight, 1f);
        itemInventoryManagerRect.anchorMin = new Vector2(0f, 1f - offsetBottom);

        itemInventoryManagerRect.sizeDelta = new Vector2(0, 0);

        ChipUI.chipCellSize = cellSize;
    }

    //[ExecuteInEditMode]
    //private void Update()
    //{
    //    gridLayout = GetComponentInChildren<GridLayoutGroup>();
    //    rectTransform = GetComponent<RectTransform>();

    //    float cellWidth = rectTransform.rect.width / gridLayout.constraintCount;
    //    float cellHeight = rectTransform.rect.height / gridLayout.constraintCount;

    //    float cellSize = Mathf.Min(cellWidth, cellHeight);

    //    gridLayout.cellSize = new Vector2(cellSize, cellSize);

    //    float exactGridSize = cellSize * gridLayout.constraintCount;

    //    float offsetRight =  exactGridSize / rectTransform.rect.width;
    //    float offsetBottom = exactGridSize / rectTransform.rect.height;

    //    itemInventoryManagerRect.anchorMax = new Vector2(offsetRight, 1f);
    //    itemInventoryManagerRect.anchorMin = new Vector2(0f, 1f - offsetBottom);

    //    itemInventoryManagerRect.sizeDelta = new Vector2(0, 0);

    //    ChipUI.chipCellSize = cellSize;
    //}
}
