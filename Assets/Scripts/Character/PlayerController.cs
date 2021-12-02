using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Movement movement;
    Combat combat;

    [SerializeField] VariableJoystick joystick;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        combat = GetComponent<Combat>();
    }

    void Update()
    {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        movement.Move(direction);
    }
}