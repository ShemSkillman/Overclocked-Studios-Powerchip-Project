using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] protected float hitPoints;
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private AudioClip deathSound;

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
                if(gameObject.tag == "Enemy")
                {
                    AudioClip clip = gameObject.gameObject.GetComponent<AudioSource>().clip;
                    Debug.Log("PLAYING");
                    gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
                   
                }
                Destroy(gameObject);
            }
        }
    }

    public bool IsDead()
    {
        return hitPoints <= 0;
    }
    
    private void OnDestroy()
    {
        if (gameObject.tag == "Enemy")
        {       
            LevelManager.EnemyCounter--;
        }
    }
}