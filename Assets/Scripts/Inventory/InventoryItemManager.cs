using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemManager : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GridLayoutGroup inventoryGrid;
    [SerializeField] InventoryNearbyItems inventoryNearby;
    [SerializeField] AudioClip chipActivate;
    [SerializeField] AudioClip chipPickup;
    AudioSource audioSource;

    Dictionary<ChipUI, Vector2Int> storedChips = new Dictionary<ChipUI, Vector2Int>();

    ChipUI draggedChip;

    bool[,] gridTakenSpaces = new bool[5,5];

    RectTransform rectTransform;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        rectTransform = GetComponent<RectTransform>();

        ChipUI.chipCellSize = inventoryGrid.cellSize.x;
    }

    void Update()
    {
        ChipUI[] chips = new ChipUI[storedChips.Count]; 
        storedChips.Keys.CopyTo(chips,0);

        foreach (ChipUI chip in chips)
        {
            if(chip.transform.parent == inventoryNearby.transform)
            {
                storedChips.Remove(chip);
            }
        }
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

        Vector2Int chipOriginGridCoords = GetClosestSlotCoodinate(chipUI.GetWorldChipCellPosition(0, 0));

        bool[,] chipMap = chipUI.itemData.chipLayoutMap.GetBoolean2DArray();

        Vector2Int closestSlotCoord = GetClosestSlotCoodinate(chipUI.GetWorldChipCellPosition(0, 0));

        if (IsChipPlacementValid(chipMap, closestSlotCoord))
        {
            //play activate sound
            audioSource.clip = chipActivate;
            audioSource.Play();
            
            PlaceChip(chipUI, chipOriginGridCoords);
        }
        else
        {
            if (storedChips.ContainsKey(chipUI))
            {
                if(IsChipPlacementValid(chipMap, storedChips[chipUI]))
                {
                    PlaceChip(chipUI, storedChips[chipUI]);
                }
            }
            else
            {
                chipUI.DesiredParent = inventoryNearby.transform;
            }
        }
    }

    void PlaceChip(ChipUI chipUI, Vector2Int chipOriginGridCoords)
    {
        bool[,] chipMap = chipUI.itemData.chipLayoutMap.GetBoolean2DArray();
        RectTransform chipRect = chipUI.GetComponent<RectTransform>();

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
        chipRect.anchoredPosition = GetSlotLocalPosition(chipOriginGridCoords.x, chipOriginGridCoords.y) - chipUI.GetLocalChipCellPosition(0,0);

        onChipMoved?.Invoke();
    }

    public delegate void OnChipMoved();

    public OnChipMoved onChipMoved;

    bool IsChipPlacementValid(bool[,] chipMap, Vector2Int chipOriginGridCoords)
    {
        bool isValid = true;

        for (int y = 0; y < chipMap.GetLength(1); y++)
        {
            for (int x = 0; x < chipMap.GetLength(0); x++)
            {
                if (chipMap[x, y] == false)
                {
                    continue;
                }

                Vector2Int currentGridCoords = new Vector2Int(chipOriginGridCoords.x + x, chipOriginGridCoords.y + y);

                if (currentGridCoords.x >= gridTakenSpaces.GetLength(0) || currentGridCoords.x < 0 || currentGridCoords.y >= gridTakenSpaces.GetLength(1) || currentGridCoords.y < 0)
                {
                    isValid = false;
                }
                else if (gridTakenSpaces[currentGridCoords.x, currentGridCoords.y] == true)
                {
                    isValid = false;
                }
            }
        }

        return isValid;
    }

    public Vector2Int GetClosestSlotCoodinate(Vector2 pos)
    {
        int slotsPerRow = Mathf.RoundToInt(rectTransform.rect.width / inventoryGrid.cellSize.x);
        int slotsPerColumn = Mathf.RoundToInt(rectTransform.rect.height / inventoryGrid.cellSize.y);

        int x = Mathf.Max(Mathf.Min(Mathf.FloorToInt((pos.x / rectTransform.rect.width) * slotsPerRow), slotsPerRow - 1), 0);
        int y = Mathf.Max(Mathf.Min(Mathf.FloorToInt(((rectTransform.rect.height + pos.y) / rectTransform.rect.height) * slotsPerColumn), slotsPerColumn -1 ), 0);

        return new Vector2Int(x, y);
    }

    public void OnStartDrag(ChipUI chipUI)
    {
        chipUI.onStartDrag -= OnStartDrag;

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

        onChipMoved?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        draggedChip.DesiredParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }

        draggedChip = eventData.pointerDrag.GetComponent<ChipUI>();

        draggedChip.DesiredParent = inventoryNearby.transform;

        draggedChip = null;
    }

    public Vector2 GetSlotLocalPosition(int x, int y)
    {
        float cellSize = inventoryGrid.cellSize.x;

        Vector2 cellCentrePos = new Vector2((x * cellSize) + (cellSize / 2.0f), -rectTransform.rect.width + (y * cellSize) + (cellSize / 2.0f));

        return cellCentrePos;
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if(eventData.pointerClick == null)
    //    {
    //        return;
    //    }

    //    eventData.pointerClick.GetComponent<ChipUI>().targetOutline.enabled = true;
    //}
}