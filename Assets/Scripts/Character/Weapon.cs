using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float baseDamage = 10;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float baseAttackRate = 1.5f;
    [SerializeField] private AnimatorOverrideController animOverride;

    [Header("Knockback")]
    [SerializeField] bool hasKnockback = false;
    [SerializeField] [Range(0f, 1f)] float verticalKnockback = 0.5f;
    [SerializeField] float knockbackForce = 20;
    [SerializeField] [Range(0f, 1f)] float knockbackRandomness = 0.1f;

    public float BaseDamage { get { return baseDamage; } }

    public float AttackRange { get { return attackRange; } }

    public float BaseAttackRate { get { return baseAttackRate; } }

    public bool HasKnockback { get { return hasKnockback; } }

    public float VerticalKnockback { get { return verticalKnockback; } }

    public float KnockbackForce { get { return knockbackForce; } }

    public float KnockbackRandomness { get { return knockbackRandomness; } }

    public AnimatorOverrideController AnimationOverride { get { return animOverride; } }
}