using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialSection : MonoBehaviour
{
    [SerializeField] string[] text;

    TextMeshPro textDisplay;

    bool isPlayerInBounds = false;

    [SerializeField] float switchTextTime = 3f;
    float timeSinceTextSwitch = 0f;

    int currentTextIndex = 0;


    private void Awake()
    {
        textDisplay = GetComponentInChildren<TextMeshPro>();

        if (text.Length > 0)
        {
            textDisplay.text = text[0];
        }        

        textDisplay.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        textDisplay.gameObject.SetActive(true);
        isPlayerInBounds = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        textDisplay.gameObject.SetActive(false);
        isPlayerInBounds = false;
    }

    void Update()
    {
        if (text.Length < 1)
        {
            return;
        }

        if (isPlayerInBounds)
        {
            timeSinceTextSwitch += Time.deltaTime;

            if (timeSinceTextSwitch >= switchTextTime)
            {
                timeSinceTextSwitch = 0f;

                currentTextIndex++;
                if (currentTextIndex >= text.Length)
                {
                    currentTextIndex = 0;
                }

                textDisplay.text = text[currentTextIndex];
            }
        }
    }
}
