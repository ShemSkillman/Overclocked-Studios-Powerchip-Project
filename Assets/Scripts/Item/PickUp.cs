using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public ItemScriptableObject item;

    private void Awake()
    {        
        item.id = System.Guid.NewGuid().ToString();
        print(item.id);
    }
}
