using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private bool isKeyboardControls = true;

    private CharacterController charController;
    private EntityStats stats;

    private Vector3 moveVector;
    public Transform lookAt;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        stats = GetComponent<EntityStats>();
    }

    private void LateUpdate()
    {
        charController.SimpleMove(moveVector * stats.MovementSpeed);

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
        if (isKeyboardControls)
        {
            this.moveVector = moveVector;
        }
        else
        {
            this.moveVector = Quaternion.AngleAxis(45, Vector3.up) * moveVector;
        }        
    }
}