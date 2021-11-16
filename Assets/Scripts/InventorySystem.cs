using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour, IDropHandler
{
    public GameObject inventoryPanel;
    public GameObject inventoryText;

    [SerializeField]
    private RectTransform ground;

    private Dictionary<string, PickUp> pickUps;

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(ground.transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                ReturnItems();
                Time.timeScale = 1f;
            } 
            else
            {
                inventoryPanel.SetActive(true);
                inventoryText.SetActive(false);
                CheckItemsInRadius(GameObject.FindGameObjectWithTag("Player").transform.position, 5f, LayerMask.GetMask("Pickup"));
                Time.timeScale = 0f;
            }
                
        }
    }

    void CheckItemsInRadius(Vector3 center, float radius, LayerMask mask)
    {
        Collider[] itemColliders = Physics.OverlapSphere(center, radius, mask);

        foreach (var itemCol in itemColliders)
        {
            ItemScriptableObject item = itemCol.GetComponent<PickUp>().item;
            //itemCol.gameObject.SetActive(false);
            Instantiate(item.inventoryChip, ground.transform);
            pickUps[item.id] = itemCol.GetComponent<PickUp>();
        }
    }

    void ReturnItems()
    {
        foreach (var id in pickUps.Keys)
        {
            if(GetGroundInventoryChips().ContainsKey(id) == false)
            {
                Destroy(pickUps[id]);
            }
        }
    }

    Dictionary<string, InventoryChip> GetGroundInventoryChips()
    {
        int count = transform.childCount;
        Dictionary<string, InventoryChip> inventoryChips = new Dictionary<string, InventoryChip>();

        for (int i = 0; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            InventoryChip chip = child.GetComponent<InventoryChip>();
            inventoryChips[chip.item.id] = chip;
        }
        return inventoryChips;
    }
}
