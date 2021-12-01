using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnTest : MonoBehaviour
{
    Movement playerMovement;
    Slider turnSlider;
    TextMeshProUGUI text;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponentInParent<Movement>();

        turnSlider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        turnSlider.value = playerMovement.TurnSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.TurnSpeed = turnSlider.value;
        text.text = "Player Turn Speed: " + turnSlider.value;
    }
}
