using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purse : MonoBehaviour
{
    [SerializeField] float startingBalance = 400f;

    float currentBalance = 0;

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
        print($"Balance: {currentBalance}");
    }
}
