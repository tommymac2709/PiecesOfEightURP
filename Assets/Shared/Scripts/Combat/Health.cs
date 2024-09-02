using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] public float currentHealth { get; private set; }

    public event Action OnTakeDamage;
    public event Action OnDie;

    public bool IsDead => currentHealth == 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Change when completing enemy setup
        if (GetComponent<BaseStats>() != null)
        {
            currentHealth = GetComponent<BaseStats>().GetHealth();
        }
        else
        {
            currentHealth = maxHealth;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetPercent()
    {
        return 100 * (currentHealth / GetComponent<BaseStats>().GetHealth());
    }

    public void DealDamage(GameObject instigator, float damage)
    {
        if (currentHealth <= 0) { return; }

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        OnTakeDamage?.Invoke();

        if (currentHealth == 0)
        {
            OnDie?.Invoke();
            AwardExperience(instigator);

        }



        Debug.Log(currentHealth);

        //the above from currentHealth-= damage can be written as:
        //currentHealth = Mathf.Max(currentHealth - damage, 0);
    }

    private void AwardExperience(GameObject instigator)
    {
        Experience experience = instigator.GetComponent<Experience>();
        Debug.Log("Attacker was " + instigator.gameObject.name);

        if (experience == null) { return; }

        experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
    }
}
