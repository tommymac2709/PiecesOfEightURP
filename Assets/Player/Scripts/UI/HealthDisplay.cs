using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    Health health;
    [SerializeField] private TextMeshProUGUI healthValueText;

    private void Awake()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        healthValueText.text = health.GetPercent().ToString("f0") + "%";
    }
}
