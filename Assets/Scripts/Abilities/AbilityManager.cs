using System;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public Ability[] abilities;
    public int currentAbilityIndex { get; private set; } = 0;

    public event Action TriggerAbility;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentAbilityIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) currentAbilityIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) currentAbilityIndex = 2;

        //if (Input.GetKeyDown(KeyCode.T)) // Example key to activate ability
        //{
        //    if (abilities[currentAbilityIndex].isActive || abilities[currentAbilityIndex].isCoolingDown) { return; }

        //    abilities[currentAbilityIndex].Activate(gameObject);
        //}
    }
}
