using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private float rateOfFire = 1.5f;

    private Animator animator;
    private float timeSinceAttack = Mathf.Infinity;

    public float AttackSpeed 
    {
        get
        {
            return rateOfFire;
        }
        set
        {
            rateOfFire = value;
        }
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        animator.SetFloat("attackSpeedMult", 1 / rateOfFire);
    }

    public void StartMeleeAttack()
    {
        if (timeSinceAttack >= rateOfFire)
        {
            animator.SetTrigger("MeleeAttack");
            timeSinceAttack = 0f;
        }        
    }

    public void MeleeHit()
    {
        weapon.Hit();
    }
}