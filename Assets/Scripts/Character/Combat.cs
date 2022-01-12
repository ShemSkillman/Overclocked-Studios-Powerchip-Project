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

    private EntityStats stats;

    private void Awake()
    {
        charController = GetComponentInParent<CharacterController>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EntityStats>();

        OverrideAnimation();
    }

    private void Start()
    {
        DisableSlashEffect();

        //animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = weapon.AnimationOverride;
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        animator.SetFloat("attackSpeedMult", 1 / GetAttackRate());
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

    private float GetAttackRate()
    {
        return Mathf.Max(minAttackRate, weapon.BaseAttackRate + stats.GetBuffAdditive(BuffType.AttackSpeed));
    }

    private Vector3 GetMeleeAttackCenter()
    {
        return charController.transform.TransformPoint(charController.center);
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
        Collider[] colliders = Physics.OverlapSphere(GetMeleeAttackCenter(), weapon.AttackRange, LayerMask.GetMask(targetLayerName));

        foreach (Collider collider in colliders)
        {
            if (IsTargetBehind(collider.transform))
            {
                continue;
            }

            BaseHealth health = collider.gameObject.GetComponentInParent<BaseHealth>();
            if (health != null)
            {
                health.TakeDamage(weapon.BaseDamage + stats.GetBuffAdditive(BuffType.AttackStrength));
            }
        }
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
        Gizmos.DrawWireSphere(GetMeleeAttackCenter(), weapon.AttackRange);
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
        if (timeSinceAttack >= GetAttackRate())
        {
            animator.SetTrigger("MeleeAttack");
            timeSinceAttack = 0f;
        }        
    }

    
}