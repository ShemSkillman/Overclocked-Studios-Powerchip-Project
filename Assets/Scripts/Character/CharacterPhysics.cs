using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysics : MonoBehaviour
{
    [SerializeField] float gravity = 80f;
    [SerializeField] float maxFallSpeed = 50f;

    [Range(0f, 0.95f)]
    [SerializeField] float horizontalDrag = 0.1f;

    [Range(0f, 1f)]
    [SerializeField] float groundFriction = 0.9f;

    [SerializeField] float knockBackResistance = 20f;
    [SerializeField] float knockBackDecay = 0.8f;

    CharacterController charController;

    // Stores desired movement for that frame depending
    //on controller input
    Vector3 desiredMovement, lastDesiredMovement;

    public Vector3 DesiredMovement { get { return desiredMovement; } }

    //Stored movement values effected by drag and gravity
    Vector3 characterVelocity;

    Coroutine knockBackProgress, dodgeProgress;

    public bool IsKnockedBack { get { return knockBackProgress != null; } }
    public bool IsDodging { get { return dodgeProgress != null; } }

    public bool IsStuck { get; private set; } = false;

    //Knockback ID prevents a knockback force from an entity
    //from being passed on to another entity more than once
    public float KnockBackID { get; private set; }

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    //When entity is not knockedback they are free to move in any direction they want
    //at a constant horizontal speed
    public void ApplyDesiredCharacterMovement()
    {
        if (knockBackProgress != null || dodgeProgress != null) return;

        ApplyGravity(); //Only external force that is applied when entity controls movement

        desiredMovement.y = characterVelocity.y; //Vertical movement not directly controlled

        var flags = charController.Move(desiredMovement * Time.deltaTime);
        ProcessFlags(flags);

        desiredMovement = Vector3.zero; //reset input movement
    }

    //Character assumed stuck when collsion on the sides of character collider
    private void ProcessFlags(CollisionFlags flags)
    {
        if (flags.HasFlag(CollisionFlags.Sides)) IsStuck = true;
        else IsStuck = false;
    }

    //Knocks back this entity by set force and blocks input for set duration
    //ID prevents same knockback force from being applied more than once to this entity
    public void KnockBack(Vector3 force, float id)
    {
        KnockBackID = id;
        if (force.magnitude <= knockBackResistance)
        {
            return; //Entity can ignore small knockback forces depending on their resistance
        }
        else
        {
            force *=  force.magnitude / (knockBackResistance + force.magnitude);
        }            

        if (knockBackProgress != null) StopCoroutine(knockBackProgress); //Reset knockback progress
        if (dodgeProgress != null) StopCoroutine(dodgeProgress);

        knockBackProgress = StartCoroutine(KnockBackProgress(force)); //Apply knockback force for duration
    }

    //Runs for duration of set knockback time while input is blocked
    IEnumerator KnockBackProgress(Vector3 force)
    {
        characterVelocity = force; //Immediately apply full knockback force

        do
        {
            charController.Move(characterVelocity * Time.deltaTime);

            ApplyGravity();
            ApplyDrag(); //Slows horizontal knockback force overtime

            yield return null;
        }
        while (characterVelocity.magnitude > knockBackResistance);

        desiredMovement = Vector3.zero; //Ensures input remains blocked and reset

        knockBackProgress = null; //Knockback progress finished
    }
    public void Dodge(float speed, float distance)
    {
        Vector3 force = lastDesiredMovement.normalized;
        if (force == Vector3.zero)
        {
            force = transform.forward;
        }

        force *= speed;

        if (dodgeProgress != null) StopCoroutine(dodgeProgress);
        dodgeProgress = StartCoroutine(DodgeProgress(force, distance));
    }

    IEnumerator DodgeProgress(Vector3 force, float distance)
    {
        Physics.IgnoreLayerCollision(6, 3, true);

        characterVelocity = force; //Immediately apply full knockback force

        transform.forward = force.normalized;

        float travelled = 0f;
        do
        {
            Vector3 moveVector = characterVelocity * Time.deltaTime;
            charController.Move(moveVector);

            travelled += moveVector.magnitude;

            yield return null;
        }
        while (travelled < distance);

        desiredMovement = Vector3.zero; //Ensures input remains blocked and reset

        dodgeProgress = null; //Knockback progress finished

        charController.detectCollisions = true;

        Physics.IgnoreLayerCollision(6, 3, false);
    }

    //Only used during knockback to slow horizontal force
    private void ApplyDrag()
    {
        characterVelocity.x *= 1 - horizontalDrag;
        characterVelocity.z *= 1 - horizontalDrag;

        if (charController.isGrounded)
        {
            //Character slows significantly faster when touching ground
            characterVelocity.x *= 1 - groundFriction;
            characterVelocity.z *= 1 - groundFriction;
        }
    }

    public bool EntityMove(Vector3 movement)
    {
        if (knockBackProgress != null) return false;

        desiredMovement = movement;
        lastDesiredMovement = movement;

        return true;
    }

    private void ApplyGravity()
    {
        if (charController.isGrounded)
        {
            characterVelocity.y = 0f;
            return;
        }

        float targetFallSpeed;

        //Prevents character from falling at full fallspeed
        //when leaving the ground
        if (charController.isGrounded) targetFallSpeed = maxFallSpeed * 0.1f; 
        else targetFallSpeed = maxFallSpeed;

        if (characterVelocity.y > -targetFallSpeed)
        {
            characterVelocity.y -= gravity * Time.deltaTime;
        }
        else //Remain at fall speed limit
        {
            characterVelocity.y = -targetFallSpeed;
        }
    }

    //Applies chain knockback to other characters this character touches
    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (knockBackProgress == null) return;
        CharacterPhysics charPhysics = other.gameObject.GetComponent<CharacterPhysics>();
        if (charPhysics == null || charPhysics.KnockBackID == KnockBackID) return;

        //Knockback decay prevents infinite chain of knockback
        charPhysics.KnockBack(characterVelocity * knockBackDecay, KnockBackID);
    }

}
