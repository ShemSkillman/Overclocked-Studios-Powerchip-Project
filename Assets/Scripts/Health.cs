using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int hitPoints = 100;

    public void TakeDamage(int dmgPoints)
    {
        hitPoints -= dmgPoints;

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool IsDead()
    {
        return hitPoints <= 0;
    }
}