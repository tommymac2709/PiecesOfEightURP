using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    BaseStats playerBaseStats;
    BaseStats enemyBaseStats;
    Targeter targeter;
    [SerializeField] private TextMeshProUGUI playerLevelValueText;
    [SerializeField] private TextMeshProUGUI enemyLevelValueText;

    private void Awake()
    {
        targeter = GameObject.FindWithTag("Player").GetComponentInChildren<Targeter>();
        playerBaseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        
    }

    private void Update()
    {
        
        playerLevelValueText.text = String.Format("{0:0}", playerBaseStats.GetLevel());

        if (targeter.GetTarget() == null)
        {
            enemyLevelValueText.text = "N/A";
            return;
        }
        enemyLevelValueText.text = targeter.GetTarget().GetComponent<BaseStats>().GetLevel().ToString();
    }
}
