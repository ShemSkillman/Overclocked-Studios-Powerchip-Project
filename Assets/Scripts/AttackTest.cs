using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackTest : MonoBehaviour
{
    Combat playerCombat;
    Slider attackSlider;
    TextMeshProUGUI text;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerCombat = player.GetComponentInParent<Combat>();

        attackSlider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackSlider.value = playerCombat.AttackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        playerCombat.AttackSpeed = Mathf.Round(attackSlider.value * 100f) / 100f;
        text.text = "Player Attack Speed: " + Mathf.Round(attackSlider.value * 100f) / 100f;
    }
}
