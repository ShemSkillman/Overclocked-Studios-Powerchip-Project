using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartMeleeAttack()
    {
        animator.SetTrigger("MeleeAttack");
    }

    public void MeleeHit()
    {
        weapon.Hit();
    }
}