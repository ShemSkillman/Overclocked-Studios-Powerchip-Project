using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeRechargeUI : MonoBehaviour
{
    [SerializeField] DodgeAbility player;
    [SerializeField] Image radialLoadingBar;
    [SerializeField] Button dodgeButton;

    private void Update()
    {
        radialLoadingBar.fillAmount = player.DodgeRechargePercentage();
        if (radialLoadingBar.fillAmount < 1f)
        {
            dodgeButton.interactable = false;
            radialLoadingBar.color = Color.yellow;
        }
        else
        {
            dodgeButton.interactable = true;
            radialLoadingBar.color = Color.green;
        }
    }
}
