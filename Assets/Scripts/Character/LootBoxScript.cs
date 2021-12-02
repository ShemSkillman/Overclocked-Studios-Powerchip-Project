using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxScript : MonoBehaviour
{
    private Animation animLootBox;
    private bool isOpen;

    public GameObject chip;

    //array of chips from folder "Chips"
    

    private Health health;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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

        //create chip array in folder "Chips"
        Object[] chipArray = Resources.LoadAll("Chips");

        //select one random chip
        Object chip = chipArray[Random.Range(0, chipArray.Length)];

        //Instantiate chip
        //Instantiate(chip, );

        Instantiate(chip, transform.position + transform.right -  transform.up * 0.5f, Quaternion.identity);
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
