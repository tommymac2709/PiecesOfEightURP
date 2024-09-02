using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [Range(1, 99)]
    [SerializeField] private int startingLevel = 1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression = null;

    public float GetStat(Stat stat)
    {
        return progression.GetStat(stat, characterClass, startingLevel);
    }
}
