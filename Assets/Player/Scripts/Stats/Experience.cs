using GameDevTV.Saving;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour, IJsonSaveable
{
    [SerializeField] float currentExperiencePoints = 0;

    public event Action onExperienceGained;

    SavingWrapper wrapper;

    private void Start()
    {
        wrapper = FindObjectOfType<SavingWrapper>();    
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            GainExperience(Time.deltaTime * 100);
        }
    }

    public void GainExperience(float experience)
    {
        currentExperiencePoints += experience;
        wrapper.SaveGame();
        onExperienceGained();

    }

    public float GetExperience()
    {
        return currentExperiencePoints;
    }

    //public object CaptureState()
    //{
    //    return currentExperiencePoints;
    //}

    //public void RestoreState(object state)
    //{
    //    currentExperiencePoints = (float)state;
    //}

    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(currentExperiencePoints);
    }

    public void RestoreFromJToken(JToken state)
    {
        currentExperiencePoints = state.ToObject<float>();

    }
}
