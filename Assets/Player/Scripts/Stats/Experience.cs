using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour, ISaveable
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

    public object CaptureState()
    {
        return currentExperiencePoints;
    }

    public void RestoreState(object state)
    {
        currentExperiencePoints = (float)state;
    }
}
