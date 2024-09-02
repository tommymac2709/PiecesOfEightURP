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

    private void Start()
    {
        targeter = GameObject.FindWithTag("Player").GetComponentInChildren<Targeter>();
        playerBaseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        
    }

    private void Update()
    {
        if (targeter.GetTarget() == null)
        {
            enemyLevelValueText.text = "N/A";
            return;
        }
        playerLevelValueText.text = playerBaseStats.GetLevel().ToString();
        enemyLevelValueText.text = targeter.GetTarget().GetComponent<BaseStats>().GetLevel().ToString();
    }
}
