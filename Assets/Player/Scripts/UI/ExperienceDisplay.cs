using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExperienceDisplay : MonoBehaviour
{
    Experience experience;
    [SerializeField] private TextMeshProUGUI experienceValueText;

    private void Awake()
    {
        experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
    }

    private void Update()
    {
        experienceValueText.text = experience.GetExperience().ToString("f0");
    }
}
