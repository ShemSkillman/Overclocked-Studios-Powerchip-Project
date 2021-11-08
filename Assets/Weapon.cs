using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float attackRate = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1f;

    private CharacterController charController;

    private void Awake()
    {
        charController = GetComponentInParent<CharacterController>();
    }

    private Vector3 GetMeleeAttackCenter()
    {
        Vector3 offset = charController.transform.forward * (charController.radius + attackRange);

        return charController.transform.TransformPoint(charController.center) + offset;
    }

    private Vector3 GetHitBox()
    {
        return new Vector3(charController.radius * 2, charController.height / 2, attackRange / 2);
    }

    public void Hit()
    {
        Collider[] colliders = Physics.OverlapSphere(GetMeleeAttackCenter(), attackRange, LayerMask.GetMask("Enemy"));

        foreach (Collider collider in colliders)
        {
            Health health = collider.gameObject.GetComponentInParent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (charController == null)
        {
            charController = GetComponentInParent<CharacterController>();
        }
        Gizmos.DrawWireSphere(GetMeleeAttackCenter(), attackRange);
    }
}