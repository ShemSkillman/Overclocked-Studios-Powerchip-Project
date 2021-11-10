using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 5f;

    private CharacterController charController;

    private Vector3 moveVector;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        charController.Move(moveVector * Time.deltaTime * moveSpeed);

        transform.forward = Vector3.RotateTowards(transform.forward, moveVector, Time.deltaTime * turnSpeed, 0.0f);
    }

    public void Move(Vector3 moveVector)
    {
        this.moveVector = moveVector;
    }
}