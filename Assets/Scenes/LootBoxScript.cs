using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxScript : MonoBehaviour
{
    private Animation animLootBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check for use

        //if player is in vicinity OR player presses E
        if(Input.GetKeyDown("e"))
        {
            OpenLootBox();
        }

    }

    void OpenLootBox()
    {
        animLootBox = GetComponent<Animation>();

        animLootBox.Play();

      
    }
}
