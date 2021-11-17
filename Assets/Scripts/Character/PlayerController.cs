using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Movement movement;
    Combat combat;

    public GameObject inventoryPanel;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        combat = GetComponent<Combat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventoryPanel.activeInHierarchy)
        {
            movement.Move(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));

            if (Input.GetMouseButtonDown(0))
            {
                combat.StartMeleeAttack();
            }
        }
    }
}