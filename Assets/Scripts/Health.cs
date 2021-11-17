using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHitpoints = 100;
    [SerializeField] private int hitPoints = 100;

    public UnityAction OnDead, OnHealthChange;

    public int Hitpoints
    {
        get
        {
            return hitPoints;
        }
    }

    public int MaxHitPoints
    {
        get
        {
            return maxHitpoints;
        }
    }

    public void TakeDamage(int dmgPoints)
    {
        hitPoints -= dmgPoints;

        OnHealthChange?.Invoke();

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
            OnDead?.Invoke();
        }
    }

    public bool IsDead()
    {
        return hitPoints <= 0;
    }
}