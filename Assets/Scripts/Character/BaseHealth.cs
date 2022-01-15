using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] protected float hitPoints;
    [SerializeField] private bool destroyOnDeath = true;
    
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    

    public UnityAction OnDead, OnHealthChange;

    public virtual float GetHitpoints()
    {
        return hitPoints;
    }

    public void TakeDamage(float dmgPoints)
    {
        hitPoints -= dmgPoints;

        //randomise and play hit sound
        randomiseAudioClip(hitSound);

        OnHealthChange?.Invoke();

        if (hitPoints <= 0)
        {
            OnDead?.Invoke();
         
            if (destroyOnDeath)
            {              
                if(gameObject.tag == "Enemy")
                {
                    AudioSource.PlayClipAtPoint(deathSound, gameObject.transform.position);                                    
                }
                Destroy(gameObject);
            }
        }
    }

    private void randomiseAudioClip(AudioClip audio)
    {

        float pitchVariation = 0.1f;
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

        audioSource.clip = audio;
        audioSource.pitch =  Random.Range(audioSource.pitch - pitchVariation, audioSource.pitch + pitchVariation);

        audioSource.Play();
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