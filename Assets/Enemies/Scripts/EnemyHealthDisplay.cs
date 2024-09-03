using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealthDisplay : MonoBehaviour
{
    Targeter targeter;

    [SerializeField] private TextMeshProUGUI healthText;


    // Start is called before the first frame update
    void Awake()
    {
        targeter = GameObject.FindWithTag("Player").GetComponentInChildren<Targeter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targeter.GetTarget() == null)
        {
            healthText.text = "N/A";
            return;
        }

        healthText.text = String.Format("{0:0}/{1:0}", targeter.GetTarget().GetComponent<Health>().GetCurrentHealth(), targeter.GetTarget().GetComponent<Health>().GetMaxHealth());
    }
}
