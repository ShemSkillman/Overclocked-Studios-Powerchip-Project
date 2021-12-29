using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    
    private Animator animator;
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
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        animator.SetFloat("attackSpeedMult", 1 / GetAttackRate());
    }

    public void EnableSlashEffect()
    {
        var emission = slashEffect.emission;
        slashEffect.gameObject.SetActive(true);
    }

    public void DisableSlashEffect()
    {
        var emission = slashEffect.emission;
        slashEffect.gameObject.SetActive(false);
    }

    public void EnableSlashEffect(bool isEnabled)
    {
        var emission = slashEffect.emission;
        emission.enabled = isEnabled;
    }

    private float GetAttackRate()
    {
        return Mathf.Max(minAttackRate, weapon.BaseAttackRate + stats.GetBuffAdditive(BuffType.AttackSpeed));
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
            BaseHealth health = collider.gameObject.GetComponentInParent<BaseHealth>();
            if (health != null)
            {
                health.TakeDamage(weapon.BaseDamage + stats.GetBuffAdditive(BuffType.AttackStrength));
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
        if (timeSinceAttack >= GetAttackRate())
        {
            animator.SetTrigger("MeleeAttack");
            timeSinceAttack = 0f;
        }        
    }
}