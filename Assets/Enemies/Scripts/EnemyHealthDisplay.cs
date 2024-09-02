using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealthDisplay : MonoBehaviour
{
    Targeter targeter;

    [SerializeField] private TextMeshProUGUI healthText;


    // Start is called before the first frame update
    void Start()
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

        healthText.text = targeter.GetTarget().GetComponent<Health>().GetPercent().ToString("f0") + "%";
    }
}
