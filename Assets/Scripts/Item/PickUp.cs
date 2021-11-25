using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public ItemScriptableObject itemData;

    private void Awake()
    {        
        itemData.ID = System.Guid.NewGuid().ToString();
        print(itemData.ID);
    }
}
