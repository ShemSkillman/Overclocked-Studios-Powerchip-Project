using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private bool isKeyboardControls = true;

    private CharacterPhysics characterPhysics;
    private EntityStats stats;

    public Transform lookAt;

    private void Awake()
    {
        characterPhysics = GetComponent<CharacterPhysics>();
        stats = GetComponent<EntityStats>();
    }

    private void LateUpdate()
    {
        Vector3 moveVector = characterPhysics.DesiredMovement;
        characterPhysics.ApplyDesiredCharacterMovement();

        if (lookAt != null)
        {
            Vector3 lookAtDir = (lookAt.position - transform.position).normalized;
            lookAtDir.y = 0;
            transform.forward = Vector3.RotateTowards(transform.forward, lookAtDir, Time.deltaTime * turnSpeed, 0.0f);
        }
        else if (moveVector != Vector3.zero)
        {
            transform.forward = Vector3.RotateTowards(transform.forward, moveVector, Time.deltaTime * turnSpeed, 0.0f);
        }
        
    }

    public void Move(Vector3 moveVector)
    {
        if (!isKeyboardControls)
        {
            moveVector = Quaternion.AngleAxis(45, Vector3.up) * moveVector;
        }

        if (moveVector != Vector3.zero)
        {
            characterPhysics.EntityMove(moveVector * stats.MovementSpeed);
        }        
    }
}