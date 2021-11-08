using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int hitPoints;

    public void TakeDamage(int dmgPoints)
    {
        hitPoints -= dmgPoints;

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}