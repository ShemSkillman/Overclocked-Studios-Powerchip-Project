using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void MeleeAttack()
    {
        animator.SetTrigger("MeleeAttack");
    }
}
