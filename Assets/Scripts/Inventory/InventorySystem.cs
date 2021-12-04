using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour, IDropHandler
{
    public GameObject inventoryPanel;

    public GameObject mainGameUI;

    [SerializeField]
    private RectTransform nearbyChips;

    [SerializeField] Transform inventoryGrid;

    [SerializeField] float pickupRadius = 5f;

    private GameObject player;
    EntityStats playerStats;

    private Dictionary<string, ChipObject> pickUps;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.tag == "InventoryItem")
        {
            eventData.pointerDrag.transform.SetParent(nearbyChips.transform);
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<EntityStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryView(!inventoryPanel.activeInHierarchy);
        }
    }

    public void ToggleInventoryView(bool isInventoryOpen)
    {
        if (isInventoryOpen)
        {
            HandleInventoryOpen();
            Time.timeScale = 0f;
        }
        else
        {
            HandleInventoryClose();
            Time.timeScale = 1f;
        }

        inventoryPanel.SetActive(isInventoryOpen);

        mainGameUI.SetActive(!isInventoryOpen);
    }

    void HandleInventoryOpen()
    {
        pickUps = new Dictionary<string, ChipObject>();

        Collider[] itemColliders = Physics.OverlapSphere(player.transform.position, pickupRadius, LayerMask.GetMask("Pickup"));

        foreach (var itemCol in itemColliders)
        {
            ChipObject pickUp = itemCol.GetComponentInParent<ChipObject>();
            CreateChipUI(pickUp);

            pickUps[pickUp.id] = pickUp;
        }
    }

    private void CreateChipUI(ChipObject pickUp)
    {
        ChipUI baseChipUI = Resources.Load<ChipUI>("Base Chip UI");
        ChipUI instance = Instantiate(baseChipUI, nearbyChips.transform);

        instance.id = pickUp.id;
        instance.itemData = pickUp.itemData;
    }

    void HandleInventoryClose()
    {
        Dictionary<string, ChipUI> nearbyInventoryChips = GetNearbyInventoryChips();

        // Destroy real world chips that are placed in inventory (no longer a nearby chip)
        foreach (string id in pickUps.Keys)
        {
            if(nearbyInventoryChips.ContainsKey(id) == false)
            {
                Destroy(pickUps[id].gameObject);
            }
        }

        // Create real world chips that are placed in nearby chips
        foreach (string id in nearbyInventoryChips.Keys)
        {
            if (pickUps.ContainsKey(id) == false)
            {
                ChipUI inventoryChip = nearbyInventoryChips[id];
                CreateChipObject(inventoryChip);                
            }
        }

        // Delete all UI nearby chips
        foreach (ChipUI inventoryChip in nearbyInventoryChips.Values)
        {
            Destroy(inventoryChip.gameObject);
        }

        playerStats.ResetBuffs();

        foreach (ChipUI equippedChip in inventoryGrid.GetComponentsInChildren<ChipUI>())
        {
            playerStats.AddBuff(equippedChip.itemData.chipBuffs);
        }
    }

    private void CreateChipObject(ChipUI chipUI)
    {
        ChipObject chipObject = Resources.Load<ChipObject>("Base Chip Object");
        ChipObject instance = Instantiate(chipObject, player.transform.position, Quaternion.identity);

        instance.id = chipUI.id;
        instance.itemData = chipUI.itemData;
    }

    Dictionary<string, ChipUI> GetNearbyInventoryChips()
    {
        int count = nearbyChips.transform.childCount;
        Dictionary<string, ChipUI> inventoryChips = new Dictionary<string, ChipUI>();

        for (int i = 0; i < count; i++)
        {
            Transform child = nearbyChips.transform.GetChild(i);
            ChipUI chip = child.GetComponent<ChipUI>();
            inventoryChips[chip.id] = chip;
        }

        return inventoryChips;
    }
}