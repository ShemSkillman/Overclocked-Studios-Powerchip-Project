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

    [SerializeField] float pickupRadius = 5f;

    private GameObject player;

    private Dictionary<string, PickUp> pickUps;

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
        pickUps = new Dictionary<string, PickUp>();

        Collider[] itemColliders = Physics.OverlapSphere(player.transform.position, pickupRadius, LayerMask.GetMask("Pickup"));

        foreach (var itemCol in itemColliders)
        {
            PickUp pickUp = itemCol.GetComponent<PickUp>();

            InventoryChip inventoryChip = Instantiate(pickUp.itemData.inventoryChip, nearbyChips.transform);
            inventoryChip.id = pickUp.id;

            pickUps[pickUp.id] = pickUp;
        }
    }

    void HandleInventoryClose()
    {
        Dictionary<string, InventoryChip> nearbyInventoryChips = GetNearbyInventoryChips();

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
                InventoryChip inventoryChip = nearbyInventoryChips[id];
                PickUp clone = Instantiate(inventoryChip.itemData.pickUp, player.transform.position, Quaternion.identity);
                clone.id = inventoryChip.id;
            }
        }

        // Delete all UI nearby chips
        foreach (InventoryChip inventoryChip in nearbyInventoryChips.Values)
        {
            Destroy(inventoryChip.gameObject);
        }
    }

    Dictionary<string, InventoryChip> GetNearbyInventoryChips()
    {
        int count = nearbyChips.transform.childCount;
        Dictionary<string, InventoryChip> inventoryChips = new Dictionary<string, InventoryChip>();

        for (int i = 0; i < count; i++)
        {
            Transform child = nearbyChips.transform.GetChild(i);
            InventoryChip chip = child.GetComponent<InventoryChip>();
            inventoryChips[chip.id] = chip;
        }

        return inventoryChips;
    }
}