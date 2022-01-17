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

    [SerializeField] bool useEnergySystem = true;

    bool isAttacking = false;
    bool isDodging = false;

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

    DodgeAbility dodge;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        combat = GetComponent<Combat>();
        animator = GetComponent<Animator>();
        dodge = GetComponent<DodgeAbility>();
    }

    void Update()
    {
        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        if (direction == Vector3.zero)
        {
            direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        }

        if (Input.GetMouseButtonDown(0))
        {
            ConstantAttack(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ConstantAttack(false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            isDodging = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDodging = false;
        }

        if (isDodging)
        {
            dodge.Dodge();
        }

        movement.Move(direction);

        if (movement.lookAt != null)
        {
            Vector3 closestPoint = movement.lookAt.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            if (Vector3.Distance(transform.position, closestPoint) > combat.GetWeaponRange())
            {
                movement.lookAt.GetComponent<AIController>().SetTargetMarkerVisibility(false);
                movement.lookAt = null;
            }
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
        Collider[] colliders = combat.GetTargetColliders();

        bool isEnemyInRange = false;
        Transform closestEnemy = null;
        float closestEnemyDist = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<AIController>() == null)
            {
                continue;
            }

            Vector3 closestPoint = collider.ClosestPointOnBounds(transform.position);
            Vector3 enemyDir = collider.transform.position - transform.position;
            float dist = Vector3.Distance(closestPoint, transform.position);

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

        if (movement.lookAt != closestEnemy)
        {
            if (movement.lookAt != null)
            {
                movement.lookAt.GetComponentInParent<AIController>().SetTargetMarkerVisibility(false);
            }

            if (closestEnemy != null)
            {
                closestEnemy.GetComponentInParent<AIController>().SetTargetMarkerVisibility(true);
            }
        }
        
        movement.lookAt = closestEnemy;
    }
}