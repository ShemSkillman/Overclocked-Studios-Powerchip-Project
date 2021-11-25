using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public ItemScriptableObject itemData;

    public string id;

    private void Awake()
    {        
        id = System.Guid.NewGuid().ToString();
    }
}
