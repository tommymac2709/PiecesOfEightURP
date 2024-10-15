using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Saving;
using Newtonsoft.Json.Linq;

public class Purse : MonoBehaviour, IJsonSaveable
{
    [SerializeField] float startingBalance = 400f;

    float currentBalance = 0;

    public event Action onChange;

    private void Awake()
    {
        currentBalance = startingBalance;
    }

    public float GetBalance()
    {
        return currentBalance;
    }

    public void UpdateBalance(float amount)
    {
        currentBalance += amount;
        if (onChange != null)
        {
            onChange();
        }
        
    }
    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(currentBalance);
    }

    public void RestoreFromJToken(JToken state)
    {
        currentBalance = state.ToObject<float>();
    }

}
