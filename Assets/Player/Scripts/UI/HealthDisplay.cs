using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

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
        healthValueText.text = String.Format("{0:0}/{1:0}", health.GetCurrentHealth(), health.GetMaxHealth());
    }
}
