using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] float currentExperiencePoints = 0;

    public event Action onExperienceGained;

    public void GainExperience(float experience)
    {
        currentExperiencePoints += experience;
        onExperienceGained();
    }

    public float GetExperience()
    {
        return currentExperiencePoints;
    }
}
