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
    [SerializeField] bool useKeyboard = false;

    bool isAttacking = false;

    float timeSinceSwivel = 0f;
    [SerializeField] float swivelRefreshRate = 0.5f;
    [SerializeField] float swivelRange = 5f;

    public void ConstantAttack(bool isAttacking)
    {
        if (!isAttacking)
        {
            animator.ResetTrigger("MeleeAttack");
        }

        this.isAttacking = isAttacking;
    }

    private void Awake()
    {
        movement = GetComponent<Movement>();
        combat = GetComponent<Combat>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 direction;
        if (useKeyboard)
        {
            direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        }
        else
        {
            direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        }

        movement.Move(direction);

        if (direction != Vector3.zero && !isAttacking)
        {
            movement.lookAt = null;
        }

        if (isAttacking)
        {
            combat.StartMeleeAttack();

            if (timeSinceSwivel >= swivelRefreshRate)
            {
                timeSinceSwivel = 0f;
                Swivel();
            }
        }

        timeSinceSwivel += Time.deltaTime;
    }

    public void Swivel()
    {
        Collider[] colliders = combat.GetTargetColliders(swivelRange);

        bool isEnemyInRange = false;
        Transform closestEnemy = null;
        float closestEnemyDist = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            Vector3 enemyDir = collider.transform.position - transform.position;
            float dist = Vector3.Distance(collider.transform.position, transform.position);

            if (closestEnemy == null)
            {
                closestEnemy = collider.transform;
                closestEnemyDist = dist;
            }
            else if (Vector3.Dot(transform.forward, enemyDir) >= 0)
            {
                if (!isEnemyInRange || dist < closestEnemyDist)
                {
                    closestEnemy = collider.transform;
                    closestEnemyDist = dist;
                }

                isEnemyInRange = true;
            }
            else if (isEnemyInRange == false && dist < closestEnemyDist)
            {
                closestEnemy = collider.transform;
                closestEnemyDist = dist;
            }
        }

        movement.lookAt = closestEnemy;
    }
}