using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : BaseHealth
{
    EntityStats stats;

    private void Awake()
    {
        stats = GetComponent<EntityStats>();
        
        if (gameObject.tag == "Enemy")
        {
            LevelManager.EnemyCounter++;
        }
    }

    public float MaxHitPoints { get { return stats.MaxHitpoints; } }

    private void Start()
    {
        hitPoints = MaxHitPoints;
    }
}