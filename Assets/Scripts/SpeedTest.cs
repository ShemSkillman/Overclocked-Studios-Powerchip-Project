using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedTest : MonoBehaviour
{
    Movement playerMovement;
    Slider speedSlider;
    TextMeshProUGUI text;


    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponentInParent<Movement>();

        speedSlider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        speedSlider.value = playerMovement.MoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement.MoveSpeed = speedSlider.value;
        text.text = "Player Move Speed: " + speedSlider.value;
    }
}
