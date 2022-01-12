using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private Image highlightedPanel;
    [SerializeField] private GameObject promptText;

    private InventorySystem inventorySystem;
    Color c;

    private void Awake()
    {
        inventorySystem = GetComponent<InventorySystem>();

        promptText.SetActive(false);

        c = highlightedPanel.color;
    }

    private void Update()
    {
        promptText.SetActive(inventorySystem.IsInventoryEmpty() && inventorySystem.IsChipNearby());

        if (inventorySystem.IsChipNearby())
        {
            if(alphaFadeInProgress == null)
            {
                alphaFadeInProgress = StartCoroutine(AlphaFade());
            }
        } else
        {
            if(alphaFadeInProgress != null)
            {
                StopCoroutine(alphaFadeInProgress);
                alphaFadeInProgress = null;
            }
        }

        if(alphaFadeInProgress == null)
        {
            Color color = c;
            color.a = 0f;
            highlightedPanel.color = color;
        }
    }

    Coroutine alphaFadeInProgress;

    IEnumerator AlphaFade()
    {
        Color currentColor = highlightedPanel.color;
        currentColor.a = 0;
        bool isFadingOut = false;

        while (true)
        {
            if (isFadingOut)
            {
                currentColor.a -= 0.01f;
            }
            else
            {
                currentColor.a += 0.01f;
            }

            highlightedPanel.color = currentColor;

            if(isFadingOut && currentColor.a <= 0)
            {
                isFadingOut = false;
                yield return new WaitForSeconds(1.0f);
            } 
            else if (!isFadingOut && currentColor.a >= c.a)
            {
                isFadingOut = true;
                yield return new WaitForSeconds(1.0f);
            }

            yield return null;
        }
    }
}
