using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float rateOfFire = 1.5f;

    public float Damage { get { return damage; } }

    public float AttackRange { get { return attackRange; } }

    public float RateOfFire { get { return rateOfFire; } }
}