using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    

    private Animator animator;
    private float timeSinceAttack = Mathf.Infinity;

    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private float minRateOfFire = 0.3f;

    private CharacterController charController;

    private EntityStats stats;

    private void Awake()
    {
        charController = GetComponentInParent<CharacterController>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EntityStats>();
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        animator.SetFloat("attackSpeedMult", 1 / ModifiedRateOfFire());
    }

    private float ModifiedRateOfFire()
    {
        return Mathf.Max(minRateOfFire, weapon.RateOfFire + stats.GetBuffAdditive(BuffType.AttackSpeed));
    }

    private Vector3 GetMeleeAttackCenter()
    {
        Vector3 offset = charController.transform.forward * (charController.radius + weapon.AttackRange);

        return charController.transform.TransformPoint(charController.center) + offset;
    }

    public void MeleeHit()
    {
        Collider[] colliders = Physics.OverlapSphere(GetMeleeAttackCenter(), weapon.AttackRange, LayerMask.GetMask(targetLayerName));

        foreach (Collider collider in colliders)
        {
            Health health = collider.gameObject.GetComponentInParent<Health>();
            if (health != null)
            {
                health.TakeDamage(weapon.Damage + stats.GetBuffAdditive(BuffType.AttackStrength));
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (charController == null)
        {
            charController = GetComponentInParent<CharacterController>();
        }
        Gizmos.DrawWireSphere(GetMeleeAttackCenter(), weapon.AttackRange);
    }

    public Collider[] GetTargetColliders()
    {
        return Physics.OverlapSphere(GetMeleeAttackCenter(), weapon.AttackRange, LayerMask.GetMask(targetLayerName));
    }

    public void StartMeleeAttack()
    {
        if (timeSinceAttack >= ModifiedRateOfFire())
        {
            animator.SetTrigger("MeleeAttack");
            timeSinceAttack = 0f;
        }        
    }
}