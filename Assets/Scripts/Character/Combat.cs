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
    AudioSource audioSource;
    [SerializeField] AudioClip weaponSwing;

    [Header("Energy System")]
    [SerializeField] bool useEnergySystem = false;
    [SerializeField] float maxEnergy = 3;
    [SerializeField] float energyGainedPerSecond = 1;
    [SerializeField] float energyUsedPerAttack = 1;
    [SerializeField] float delayBeforeRecharge = 1f;

    float currentEnergy;

    public float MaxEnergy { get { return maxEnergy; } }
    public float CurrentEnergy { get { return currentEnergy; } }

    private CharacterController charController;
    private CharacterPhysics characterPhysics;

    private EntityStats stats;

    private void Awake()
    {
        charController = GetComponentInParent<CharacterController>();
        characterPhysics = GetComponentInParent<CharacterPhysics>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EntityStats>();
        
        currentEnergy = maxEnergy;

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

        if (timeSinceAttack >= delayBeforeRecharge)
        {
            currentEnergy = Mathf.Min(maxEnergy, currentEnergy + (Time.deltaTime * energyGainedPerSecond));
        }

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

                    if (characterPhysics != null)
                    {
                        health.GetComponent<CharacterPhysics>().KnockBack(knockBackForce, Time.time);
                    }                    
                }
                
            }
        }
        //create hit effect
        if(weapon.HitEffect != null)
        {
            Instantiate(weapon.HitEffect, GetMeleeAttackCenter(), Quaternion.identity);
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
        if (characterPhysics.IsKnockedBack || characterPhysics.IsDodging || currentEnergy < energyUsedPerAttack)
        {
            return;
        }

        if (timeSinceAttack >= GetAttackRate() && currentEnergy >= energyUsedPerAttack)
        {
            

            currentEnergy -= energyUsedPerAttack;

            animator.SetTrigger("MeleeAttack");
            audioSource.clip = weaponSwing;
            audioSource.Play();
            
            timeSinceAttack = 0f;
        }        
    }

    
}