using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Purse : MonoBehaviour
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
}
