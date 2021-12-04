using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitLevel : MonoBehaviour
{
    public UnityAction OnPlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            OnPlayerEnter.Invoke();
        }
    }
}
