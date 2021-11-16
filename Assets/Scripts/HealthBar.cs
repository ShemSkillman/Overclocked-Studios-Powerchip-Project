using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Health health;
    Slider slider;
    Canvas canvas;

    [SerializeField]
    private float visibleTimeSeconds = 3f;

    float timeSinceHealthChangeSeconds = Mathf.Infinity;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
        slider = GetComponent<Slider>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        slider.maxValue = health.MaxHitPoints;
        slider.value = health.Hitpoints;
    }

    private void Update()
    {
        if (timeSinceHealthChangeSeconds < visibleTimeSeconds)
        {
            timeSinceHealthChangeSeconds += Time.deltaTime;
            canvas.enabled = true;
        }
        else
        {
            canvas.enabled = false;
        }
    }

    private void OnEnable()
    {
        health.OnHealthChange += UpdateHealthBar;
    }

    private void OnDisable()
    {
        health.OnHealthChange -= UpdateHealthBar;
    }

    private void UpdateHealthBar()
    {
        timeSinceHealthChangeSeconds = 0f;
        slider.value = health.Hitpoints;
    }
}
