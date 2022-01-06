using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float dropChance = 0.1f;

    BaseHealth health;

    private void Awake()
    {
        health = GetComponent<BaseHealth>();
    }

    private void OnEnable()
    {
        health.OnDead += DropChip;
    }

    private void OnDisable()
    {
        health.OnDead -= DropChip;
    }

    public void DropChip()
    {
        if (Random.value > dropChance)
        {
            return;
        }

        ChipObject chipObject = Resources.Load<ChipObject>("Base Chip Object");
        ChipObject instance = Instantiate(chipObject, transform.position, Quaternion.identity);

        //create chip array in folder "Chip Data"
        ItemScriptableObject[] chipArray = Resources.LoadAll<ItemScriptableObject>("Chip Data");

        //select one random chip
        ItemScriptableObject chipData = chipArray[Random.Range(0, chipArray.Length)];

        instance.itemData = chipData;
    }
}