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
            if (inventoryPanel.activeInHierarchy)
            {
                inventoryPanel.SetActive(false);
                inventoryText.SetActive(true);
                speedText.SetActive(true);
                ReturnItems();
                Time.timeScale = 1f;
            } 
            else
            {
                inventoryPanel.SetActive(true);
                inventoryText.SetActive(false);
                speedText.SetActive(false);
                CheckItemsInRadius(player.transform.position, 5f, LayerMask.GetMask("Pickup"));
                Time.timeScale = 0f;
            }
                
        }
    }

    void CheckItemsInRadius(Vector3 center, float radius, LayerMask mask)
    {
        pickUps = new Dictionary<string, PickUp>();

        Collider[] itemColliders = Physics.OverlapSphere(center, radius, mask);

        foreach (var itemCol in itemColliders)
        {
            ItemScriptableObject item = itemCol.GetComponent<PickUp>().item;
            Instantiate(item.inventoryChip, ground.transform);
            pickUps[item.id] = itemCol.GetComponent<PickUp>();
        }
    }

    void ReturnItems()
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
        foreach (var id in GetGroundInventoryChips().Keys)
        {
            if (pickUps.ContainsKey(id) == false)
            {
                InventoryChip inventoryChip = GetGroundInventoryChips()[id];
                Instantiate(inventoryChip.item.pickUp, player.transform.position, Quaternion.identity);
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
            inventoryChips[chip.item.id] = chip;
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