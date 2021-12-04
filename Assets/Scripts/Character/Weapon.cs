using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float baseDamage = 10;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float baseAttackRate = 1.5f;

    public float BaseDamage { get { return baseDamage; } }

    public float AttackRange { get { return attackRange; } }

    public float BaseAttackRate { get { return baseAttackRate; } }
}