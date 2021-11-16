using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject inventoryPanel;

    public GameObject[] slots;

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
