using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private float hitPoints;
    [SerializeField] private bool destroyOnDeath = true;

    public UnityAction OnDead, OnHealthChange;

    EntityStats stats;

    private void Awake()
    {
        stats = GetComponent<EntityStats>();
    }

    public float Hitpoints { get { return hitPoints; } }

    public float MaxHitPoints { get { return stats.MaxHitpoints; } }

    private void Start()
    {
        hitPoints = MaxHitPoints;
    }

    public void TakeDamage(float dmgPoints)
    {
        hitPoints -= dmgPoints;

        OnHealthChange?.Invoke();

        if (hitPoints <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            OnDead?.Invoke();
        }
    }

    public bool IsDead()
    {
        return hitPoints <= 0;
    }
}