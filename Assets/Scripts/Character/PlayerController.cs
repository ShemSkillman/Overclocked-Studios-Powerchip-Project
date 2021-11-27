using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Movement movement;
    Combat combat;

    [SerializeField] VariableJoystick joystick;

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
            Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
            movement.Move(direction);

            //if (Input.GetMouseButtonDown(0))
            //{
            //    combat.StartMeleeAttack();
            //}
        }
    }
}