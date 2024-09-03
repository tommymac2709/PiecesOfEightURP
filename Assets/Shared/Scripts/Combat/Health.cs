using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    //Used in health regeneration to regenerate health on level up to percentage of new level max health
    [SerializeField] float regenerationPercentage = 70f;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] public float currentHealth { get; private set; }

    public event Action OnTakeDamage;
    public event Action OnDie;

    public bool IsDead => currentHealth == 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<BaseStats>().onLevelUp += RegenerateHealth;

        //Change when completing enemy setup
        if (GetComponent<BaseStats>() != null)
        {
            currentHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
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
        return 100 * (currentHealth / GetComponent<BaseStats>().GetStat(Stat.Health));
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

        experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
    }

    private void RegenerateHealth()
    {
        //Regen to percentage of the new level max health if below that number
        //float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
        //currentHealth = Mathf.Max(currentHealth, regenHealthPoints);

        

        //Regenerate to max health
        currentHealth = GetComponent<BaseStats>().GetStat(Stat.Health);

        Debug.Log(currentHealth);
    }
}
