using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridLayoutScaler : MonoBehaviour
{
    private GridLayoutGroup gridLayout;
    RectTransform rectTransform;
    bool isFlexible;

    private void Start()
    {
        gridLayout = GetComponentInChildren<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        float cellWidth = rectTransform.rect.width / gridLayout.constraintCount;
        float cellHeight = rectTransform.rect.height / gridLayout.constraintCount;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
}
