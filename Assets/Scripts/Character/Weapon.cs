using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1f;

    [SerializeField] private string targetLayerName = "Player";

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

    public void Hit()
    {
        Collider[] colliders = Physics.OverlapSphere(GetMeleeAttackCenter(), attackRange, LayerMask.GetMask(targetLayerName));

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

    public Collider[] GetTargetColliders()
    {
        return Physics.OverlapSphere(GetMeleeAttackCenter(), attackRange, LayerMask.GetMask(targetLayerName));
    }
}