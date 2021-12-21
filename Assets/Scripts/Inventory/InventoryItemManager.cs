using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemManager : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Transform inventoryGrid;
    [SerializeField] InventoryNearbyItems inventoryNearby;

    Dictionary<ChipUI, Vector2Int> storedChips = new Dictionary<ChipUI, Vector2Int>();

    bool[,] gridTakenSpaces = new bool[5,5];

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
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
        chipRect.transform.SetParent(transform);

        Vector2Int firstChipCellGridCoords = GetClosestSlotCoodinate(chipUI.GetFirstChipCellPosition(true));

        Vector2Int chipOriginGridCoords = GetClosestSlotCoodinate(chipUI.GetWorldChipCellPosition(0, 0));

        bool[,] chipMap = chipUI.itemData.chipLayoutMap.GetBoolean2DArray();

        bool isValid = true;
        for (int y = 0; y < chipMap.GetLength(1); y++)
        {
            for (int x = 0; x < chipMap.GetLength(0); x++)
            {
                if (chipMap[x,y] == false)
                {
                    continue;
                }

                Vector2Int currentGridCoords = new Vector2Int(chipOriginGridCoords.x + x, chipOriginGridCoords.y + y);

                //print("X = " + currentGridCoords.x + " Y = " + currentGridCoords.y);

                if (currentGridCoords.x >= gridTakenSpaces.GetLength(0) || currentGridCoords.x < 0 || currentGridCoords.y >= gridTakenSpaces.GetLength(1) || currentGridCoords.y < 0)
                {
                    print("out of bounds!");
                    isValid = false;
                }
                else if (gridTakenSpaces[currentGridCoords.x, currentGridCoords.y] == true)
                {
                    print("space occupied!");
                    isValid = false;
                }
            }
        }

        if (isValid)
        {
            for (int y = 0; y < chipMap.GetLength(1); y++)
            {
                for (int x = 0; x < chipMap.GetLength(0); x++)
                {
                    if (chipMap[x, y] == false)
                    {
                        continue;
                    }

                    gridTakenSpaces[chipOriginGridCoords.x + x, chipOriginGridCoords.y + y] = true;
                }
            }

            chipUI.onStartDrag += OnStartDrag;
            storedChips[chipUI] = chipOriginGridCoords;
            chipRect.anchoredPosition = GetSlotLocalPosition(firstChipCellGridCoords.x, firstChipCellGridCoords.y) - chipUI.GetFirstChipCellPosition(false);
        }
        else
        {
            chipUI.DesiredParent = inventoryNearby.transform;
        }
    }

    public Vector2Int GetClosestSlotCoodinate(Vector2 pos)
    {
        int x = Mathf.Max(Mathf.Min(Mathf.FloorToInt((pos.x / 805f) * 5), 4), 0);
        int y = Mathf.Max(Mathf.Min(Mathf.FloorToInt(((805f + pos.y) / 805f) * 5), 4), 0);

        return new Vector2Int(x, y);
    }

    public void OnStartDrag(ChipUI chipUI)
    {
        chipUI.onStartDrag -= OnStartDrag;

        print(chipUI.itemData.itemName + " is being dragged!");

        Vector2Int chipOriginGridCoords = storedChips[chipUI];

        bool[,] chipMap = chipUI.itemData.chipLayoutMap.GetBoolean2DArray();

        for (int y = 0; y < chipMap.GetLength(1); y++)
        {
            for (int x = 0; x < chipMap.GetLength(0); x++)
            {
                if (chipMap[x, y] == false)
                {
                    continue;
                }

                gridTakenSpaces[chipOriginGridCoords.x + x, chipOriginGridCoords.y + y] = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        ChipUI draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        draggedChip.DesiredParent = transform;

        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).GetComponent<Image>().raycastTarget = false;
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        ChipUI draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        draggedChip.DesiredParent = inventoryNearby.transform;

        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).GetComponent<Image>().raycastTarget = true;
        //}
    }

    public Vector2 GetSlotLocalPosition(int x, int y)
    {
        Vector2 cellCentrePos = new Vector2((x * 161) + (161 / 2.0f), -805f + (y * 161) + (161 / 2.0f));

        return cellCentrePos;
    }
}