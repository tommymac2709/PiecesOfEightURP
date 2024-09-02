using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] float currentExperiencePoints = 0;

    public void GainExperience(float experience)
    {
        currentExperiencePoints += experience;
    }

    public float GetExperience()
    {
        return currentExperiencePoints;
    }
}
