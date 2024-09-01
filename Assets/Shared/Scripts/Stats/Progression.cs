using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "New Progression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField] ProgressionCharacterClass[] characterClasses = null;

    public int GetHealth(CharacterClass characterClass, int level) 
    {
        foreach (ProgressionCharacterClass progressionClass in characterClasses)
        {
            if (progressionClass.characterClass == characterClass)
            {
                return progressionClass.health[level - 1];
            }
        }
        
        return 0;
    }

    [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public int[] health;
    }
}