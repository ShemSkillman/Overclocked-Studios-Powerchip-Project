using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Movement movement;
    Combat combat;
    Animator animator;

    [SerializeField] VariableJoystick joystick;

    bool isAttacking = false;

    public void ConstantAttack(bool isAttacking)
    {
        this.isAttacking = isAttacking;

        if (!isAttacking)
        {
            animator.ResetTrigger("MeleeAttack");
        }
    }

    private void Awake()
    {
        movement = GetComponent<Movement>();
        combat = GetComponent<Combat>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        movement.Move(direction);

        if (isAttacking)
        {
            combat.StartMeleeAttack();
        }
    }
}