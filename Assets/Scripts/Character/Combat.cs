using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private Animator animator;
    protected AnimatorOverrideController animatorOverrideController;

    private float timeSinceAttack = Mathf.Infinity;

    [SerializeField] private string targetLayerName = "Player";
    [SerializeField] private float minAttackRate = 0.3f;
    [SerializeField] ParticleSystem slashEffect;

    private CharacterController charController;
    private CharacterPhysics characterPhysics;

    private EntityStats stats;

    private void Awake()
    {
        charController = GetComponentInParent<CharacterController>();
        characterPhysics = GetComponentInParent<CharacterPhysics>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EntityStats>();

        OverrideAnimation();
    }

    private void Start()
    {
        DisableSlashEffect();

        animator.runtimeAnimatorController = weapon.AnimationOverride;
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        animator.SetFloat("attackSpeedMult", 1 / GetAttackRate());

        if (characterPhysics.IsKnockedBack || characterPhysics.IsDodging)
        {
            DisableSlashEffect();
        }
    }

    private void OverrideAnimation()
    {
        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (weapon.AnimationOverride != null)
        {
            animator.runtimeAnimatorController = weapon.AnimationOverride;
        }
        else if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }
    }

    public void EnableSlashEffect()
    {
        if (slashEffect == null)
        {
            return;
        }

        var emission = slashEffect.emission;
        slashEffect.gameObject.SetActive(true);
    }

    public void DisableSlashEffect()
    {
        if (slashEffect == null)
        {
            return;
        }

        var emission = slashEffect.emission;
        slashEffect.gameObject.SetActive(false);
    }

    public float GetAttackRate()
    {
        return Mathf.Max(minAttackRate, weapon.BaseAttackRate);
    }

    private Vector3 GetMeleeAttackCenter()
    {
        return charController.transform.TransformPoint(charController.center + weapon.AttackOffset);
    }

    public float GetBaseWeaponDamage
    {
        get
        {
            return weapon.BaseDamage;
        }
    }

    public void MeleeHit()
    {
        Collider[] colliders = Physics.OverlapSphere(GetMeleeAttackCenter(), weapon.AttackRange, LayerMask.GetMask("Player", "Enemy"));

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == gameObject ||
                IsTargetBehind(collider.transform))
            {
                continue;
            }

            BaseHealth health = collider.gameObject.GetComponentInParent<BaseHealth>();
            if (health != null)
            {
                CharacterPhysics charPhysics = collider.GetComponent<CharacterPhysics>();
                if ((charPhysics == null || !charPhysics.IsDodging) &&
                    collider.gameObject.layer != gameObject.layer)
                {
                    health.TakeDamage(weapon.BaseDamage + stats.GetBuffAdditive(BuffType.AttackStrength));
                }

                if (!health.IsDead() && weapon.HasKnockback)
                {
                    Vector3 dir = (collider.transform.position - transform.position).normalized;
                    dir.y = weapon.VerticalKnockback;

                    Vector3 knockBackForce = dir * weapon.KnockbackForce * Random.Range(1f, 1f + weapon.KnockbackRandomness); //Generate random magnitude

                    health.GetComponent<CharacterPhysics>().KnockBack(knockBackForce, Time.time);
                }
                
            }
        }

        Instantiate(weapon.HitEffect, GetMeleeAttackCenter(), Quaternion.identity);
    }

    public bool IsTargetBehind(Transform target)
    {
        Vector3 enemyDir = target.position - transform.position;

        if (Vector3.Dot(transform.forward, enemyDir) < 0)
        {
            return true;
        }

        return false;
    }

    public float GetWeaponRange()
    {
        return weapon.AttackRange;
    }

    private void OnDrawGizmos()
    {
        if (charController == null)
        {
            charController = GetComponentInParent<CharacterController>();
        }

        if (charController != null && weapon != null)
        {
            Gizmos.DrawWireSphere(GetMeleeAttackCenter(), weapon.AttackRange);
        }        
    }

    public Collider[] GetTargetColliders()
    {
        return GetTargetColliders(weapon.AttackRange);
    }

    public Collider[] GetTargetColliders(float range)
    {
        return Physics.OverlapSphere(GetMeleeAttackCenter(), range, LayerMask.GetMask(targetLayerName));
    }

    public void StartMeleeAttack()
    {
        if (characterPhysics.IsKnockedBack || characterPhysics.IsDodging)
        {
            return;
        }

        if (timeSinceAttack >= GetAttackRate())
        {
            animator.SetTrigger("MeleeAttack");
            timeSinceAttack = 0f;
        }        
    }

    
}