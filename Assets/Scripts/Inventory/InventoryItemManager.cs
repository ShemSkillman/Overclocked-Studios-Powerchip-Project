using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemManager : MonoBehaviour
{
    [SerializeField] Transform inventoryGrid;
    [SerializeField] InventoryNearbyItems inventoryNearby;

    bool[,] gridTakenSpaces = new bool[5,5];

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < inventoryGrid.childCount; i++)
        {
            InventorySlot slot = inventoryGrid.GetChild(i).gameObject.GetComponent<InventorySlot>();

            slot.OnSlotDrop += OnDrop;
            slot.OnSlotEnter += OnEnter;
            slot.OnSlotExit += OnExit;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < inventoryGrid.childCount; i++)
        {
            InventorySlot slot = inventoryGrid.GetChild(i).gameObject.GetComponent<InventorySlot>();

            slot.OnSlotDrop -= OnDrop;
            slot.OnSlotEnter -= OnEnter;
            slot.OnSlotExit -= OnExit;
        }
    }

    public void OnDrop(InventorySlot slot, PointerEventData eventData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().raycastTarget = true;
        }

        if (eventData.pointerDrag == null)
        {
            return;
        }

        ChipUI chipUI = eventData.pointerDrag.GetComponent<ChipUI>();
        RectTransform chipRect = eventData.pointerDrag.GetComponent<RectTransform>();

        // Parent chip to item manager
        chipRect.transform.parent = transform;

        Vector2 firstChipCellPos = chipUI.GetFirstChipCellPosition(true);

        int xGrid = Mathf.Max(Mathf.Min(Mathf.FloorToInt((firstChipCellPos.x / 805f) * 5), 4), 0);
        int yGrid = Mathf.Max(Mathf.Min(Mathf.FloorToInt(Mathf.Abs((firstChipCellPos.y / 805f) * 5)), 4), 0);        

        bool[,] chipMap = chipUI.itemData.chipLayoutMap.GetBoolean2DArray();

        for (int y = 0; y < chipMap.GetLength(1); y++)
        {
            for (int x = 0; x < chipMap.GetLength(0); x++)
            {
                if (chipMap[x,y] == false)
                {
                    continue;
                }

                int newGridX = xGrid + x;
                int newGridY = yGrid - y;

                print("X = " + newGridX + " Y = " + newGridY);

                if (newGridX >= gridTakenSpaces.GetLength(0) || newGridX < 0 || newGridY >= gridTakenSpaces.GetLength(1) || newGridY < 0)
                {
                    print("out of bounds!");
                    goto Exit;
                }

                if (gridTakenSpaces[newGridX, newGridY] == true)
                {
                    print("space occupied!");
                    goto Exit;
                }
            }
        }

        for (int y = 0; y < chipMap.GetLength(1); y++)
        {
            for (int x = 0; x < chipMap.GetLength(0); x++)
            {
                if (chipMap[x, y] == false)
                {
                    continue;
                }

                gridTakenSpaces[xGrid + x, yGrid - y] = true;
            }
        }

    Exit:
        chipRect.anchoredPosition = GetSlotLocalPosition(xGrid, yGrid) - chipUI.GetFirstChipCellPosition(false);
    }

    public Vector2 GetSlotLocalPosition(int x, int y)
    {
        Vector2 cellCentrePos = new Vector2((x * 161) + (161 / 2.0f), (-y * 161) + (-161 / 2.0f));

        return cellCentrePos;
    }

    public void OnEnter(InventorySlot slot, PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        ChipUI draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        draggedChip.DesiredParent = transform;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().raycastTarget = false;
        }
    }

    public void OnExit(InventorySlot slot, PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        ChipUI draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        draggedChip.DesiredParent = inventoryNearby.transform;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().raycastTarget = true;
        }
    }
}
