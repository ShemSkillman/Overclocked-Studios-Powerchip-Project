using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float baseDamage = 10;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float baseAttackRate = 1.5f;

    Animator animator;
    [SerializeField] ParticleSystem slashEffect;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if(slashEffect == null)
        {
            return;
        }

        //bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");

        //var emission = slashEffect.emission;

        //emission.enabled = isAttacking;
    }

    public void EnableSlashEffect(bool isEnabled)
    {
        var emission = slashEffect.emission;
        emission.enabled = isEnabled;
    }

    public float BaseDamage { get { return baseDamage; } }

    public float AttackRange { get { return attackRange; } }

    public float BaseAttackRate { get { return baseAttackRate; } }
}