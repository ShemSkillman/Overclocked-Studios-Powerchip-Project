using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipObject : MonoBehaviour
{
    public ItemScriptableObject itemData;

    public string id;

    private void Awake()
    {        
        id = System.Guid.NewGuid().ToString();        
    }

    private void Start()
    {
        Instantiate(itemData.chipModel, transform);
    }
}
