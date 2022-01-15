using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridLayoutScaler : MonoBehaviour
{
    private GridLayoutGroup gridLayout;
    RectTransform rectTransform;

    [SerializeField] RectTransform itemInventoryManagerRect;

    private void Awake()
    {
        
    }

    private void Update()
    {
        gridLayout = GetComponentInChildren<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        float cellWidth = rectTransform.rect.width / gridLayout.constraintCount;
        float cellHeight = rectTransform.rect.height / gridLayout.constraintCount;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        float exactGridSize = cellSize * gridLayout.constraintCount;

        float offsetRight =  exactGridSize / rectTransform.rect.width;
        float offsetBottom = exactGridSize / rectTransform.rect.height;

        itemInventoryManagerRect.sizeDelta = new Vector2(offsetRight, offsetBottom);
    }
}
