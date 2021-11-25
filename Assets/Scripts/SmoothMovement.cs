using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMovement : MonoBehaviour
{
    private GameObject cardController;

    [SerializeField] float smoothing = 10f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    private void Update()
    {
        Move();
    }

    // Smoothly moves gameobject towards target card controller
    private void Move()
    {
        Vector3 targetPos = cardController.transform.position; 
        this.transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothing * Time.deltaTime); 
    }

    public GameObject CardController
    {
        set
        {
            cardController = value;
        }
    }
}
