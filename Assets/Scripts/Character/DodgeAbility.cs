using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeAbility : MonoBehaviour
{
    private CharacterPhysics characterPhysics;
    private EntityStats stats;

    [SerializeField] float dodgeSpeed = 50f;
    [SerializeField] float dodgeDistance = 5f;
    [SerializeField] AudioClip dodgeSound;
    AudioSource audioSource;

    float timeSinceLastDodge = Mathf.Infinity;
    

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        characterPhysics = GetComponent<CharacterPhysics>();
        stats = GetComponent<EntityStats>();
    }

    private void Update()
    {
        timeSinceLastDodge += Time.deltaTime;
    }

    public void Dodge()
    {
        if (timeSinceLastDodge >= stats.DodgeRechargeTime)
        {
            audioSource.clip = dodgeSound;
            audioSource.Play();

            characterPhysics.Dodge(dodgeSpeed, dodgeDistance);
            timeSinceLastDodge = 0f;
        }        
    }

    public float DodgeRechargePercentage()
    {
        return Mathf.Min(1f, timeSinceLastDodge / stats.DodgeRechargeTime);
    }
}
