using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitLevel : MonoBehaviour
{
    public UnityAction OnPlayerEnter;
    [SerializeField] AudioClip audioClip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            OnPlayerEnter.Invoke();
        }
        
        if(other.gameObject.tag == "Enemy")
        {
            AudioSource.PlayClipAtPoint(audioClip, other.gameObject.transform.position);
            Destroy(other.gameObject);
        }
    }
}
