using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] int currentExperiencePoints = 0;

    public void GainExperience(int experience)
    {
        currentExperiencePoints += experience;
    }
}
