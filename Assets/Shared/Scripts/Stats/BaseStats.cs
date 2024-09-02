using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [Range(1, 99)]
    [SerializeField] private int startingLevel = 1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression = null;

<<<<<<< HEAD
=======
    private void Update()
    {
        if (gameObject.tag == "Player")
        {
            print(GetLevel());
        }
        
    }

>>>>>>> parent of 63d6077 (Updated starting level)
    public float GetStat(Stat stat)
    {
        return progression.GetStat(stat, characterClass, startingLevel);
    }
}
