using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int hitPoints = 100;

    public UnityAction OnDead;

    public void TakeDamage(int dmgPoints)
    {
        hitPoints -= dmgPoints;

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
            OnDead.Invoke();
        }
    }

    public bool IsDead()
    {
        return hitPoints <= 0;
    }
}