using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxScript : MonoBehaviour
{
    private Animation animLootBox;
    private bool isOpen;

    public GameObject chip;

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
        //Check for use

        //if player is in vicinity OR player presses E
        //if()
        //{
        //    if (!isOpen)
        //    {
        //        OpenLootBox();
        //        isOpen = true;
        //    }
        //}
    }

    void OpenLootBox()
    {
        animLootBox = GetComponent<Animation>();

        animLootBox.Play();

        Instantiate(chip, new Vector3(transform.position.x + 1, transform.position.y - 1, transform.position.z), Quaternion.identity);

        health.OnDead -= OpenLootBox;
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
