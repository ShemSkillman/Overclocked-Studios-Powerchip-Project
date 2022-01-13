using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float baseDamage = 10;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float zOffset = 0f;
    [SerializeField] private float baseAttackRate = 1.5f;
    [SerializeField] private AnimatorOverrideController animOverride;

    public float BaseDamage { get { return baseDamage; } }

    public float AttackRange { get { return attackRange; } }

    public float ZOffset { get { return zOffset; } }

    public float BaseAttackRate { get { return baseAttackRate; } }

    public AnimatorOverrideController AnimationOverride { get { return animOverride; } }
}