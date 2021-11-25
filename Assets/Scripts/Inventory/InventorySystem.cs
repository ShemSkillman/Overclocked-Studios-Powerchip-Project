using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour, IDropHandler
{
    public GameObject inventoryPanel;
    public GameObject inventoryText;
    public GameObject speedText;

    [SerializeField]
    private RectTransform ground;

    [SerializeField] float pickupRadius = 5f;

    private GameObject player;


    private Dictionary<string, PickUp> pickUps;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null && eventData.pointerDrag.tag == "InventoryItem")
        {
            eventData.pointerDrag.transform.SetParent(ground.transform);
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryView(!inventoryPanel.activeInHierarchy);
        }
    }

    private void ToggleInventoryView(bool isInventoryOpen)
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

        inventoryText.SetActive(!isInventoryOpen);
        speedText.SetActive(!isInventoryOpen);        
    }

    void HandleInventoryOpen()
    {
        pickUps = new Dictionary<string, PickUp>();

        Collider[] itemColliders = Physics.OverlapSphere(player.transform.position, pickupRadius, LayerMask.GetMask("Pickup"));

        foreach (var itemCol in itemColliders)
        {
            PickUp pickUp = itemCol.GetComponent<PickUp>();
            ItemScriptableObject item = pickUp.itemData;

            InventoryChip clone = Instantiate(item.inventoryChip, ground.transform);
            clone.item.ID = item.ID;

            print(clone.item.ID);

            pickUps[item.ID] = pickUp;
        }
    }

    void HandleInventoryClose()
    {
        // Destroy real world items that are placed in inventory
        foreach (var id in pickUps.Keys)
        {
            if(GetGroundInventoryChips().ContainsKey(id) == false)
            {
                Destroy(pickUps[id].gameObject);
            }
        }

        // Create real world placed in ground section of inventory
        foreach (string id in GetGroundInventoryChips().Keys)
        {
            if (pickUps.ContainsKey(id) == false)
            {
                InventoryChip inventoryChip = GetGroundInventoryChips()[id];
                PickUp clone = Instantiate(inventoryChip.item.pickUp, player.transform.position, Quaternion.identity);
                clone.itemData.ID = id;
            }
        }

        // Delete all ground UI chips
        foreach (InventoryChip inventoryChip in GetGroundInventoryChips().Values)
        {
            Destroy(inventoryChip.gameObject);
        }

        //DeleteGround();
    }

    Dictionary<string, InventoryChip> GetGroundInventoryChips()
    {
        int count = ground.transform.childCount;
        Dictionary<string, InventoryChip> inventoryChips = new Dictionary<string, InventoryChip>();

        for (int i = 0; i < count; i++)
        {
            Transform child = ground.transform.GetChild(i);
            InventoryChip chip = child.GetComponent<InventoryChip>();
            inventoryChips[chip.item.ID] = chip;
        }

        return inventoryChips;
    }

    void DeleteGround()
    {
        int count = ground.transform.childCount;
        GameObject[] toDelete = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            Transform child = ground.transform.GetChild(i);
            toDelete[i] = child.gameObject;
        }

        for (int i = 0; i < count; i++)
        {
            Destroy(toDelete[i]);
        }
    }
}