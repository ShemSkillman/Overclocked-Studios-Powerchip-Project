using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] protected float hitPoints;
    [SerializeField] private bool destroyOnDeath = true;

    public UnityAction OnDead, OnHealthChange;

    public float Hitpoints { get { return hitPoints; } }

    public void TakeDamage(float dmgPoints)
    {
        hitPoints -= dmgPoints;

        OnHealthChange?.Invoke();

        if (hitPoints <= 0)
        {
            OnDead?.Invoke();
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
        }
    }

    public bool IsDead()
    {
        return hitPoints <= 0;
    }
}