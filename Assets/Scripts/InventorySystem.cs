using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour, IDropHandler
{
    public GameObject inventoryPanel;

    [SerializeField]
    private RectTransform ground;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryPanel.activeInHierarchy)
                inventoryPanel.SetActive(false);
            else
                inventoryPanel.SetActive(true);
        }
    }
}
