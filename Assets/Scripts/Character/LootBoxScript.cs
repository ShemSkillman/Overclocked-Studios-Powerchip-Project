using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxScript : MonoBehaviour
{
    private Animation animLootBox;
    
    private BaseHealth health;

    private void Awake()
    {
        health = GetComponentInParent<BaseHealth>();
    }

    void OpenLootBox()
    {
        //play opening animation
        animLootBox = GetComponent<Animation>();
        animLootBox.Play();

        //generate Random chip
        GenerateChip();

        health.OnDead -= OpenLootBox;
    }

    void GenerateChip()
    {
        ChipObject chipObject = Resources.Load<ChipObject>("Base Chip Object");
        ChipObject instance = Instantiate(chipObject, transform.position + transform.right - transform.up * 0.5f, Quaternion.identity);

        //create chip array in folder "Chip Data"
        ItemScriptableObject[] chipArray = Resources.LoadAll<ItemScriptableObject>("Chip Data");

        //select one random chip
        ItemScriptableObject chipData = chipArray[Random.Range(0, chipArray.Length)];

        instance.itemData = chipData;
    }

    private void OnEnable()
    {
        health.OnDead += OpenLootBox;
    }

    private void OnDisable()
    {
        health.OnDead -= OpenLootBox;
    }
}
