using System;
using UnityEngine;
using GameDevTV.Utils;
using System.Collections.Generic;

public class Health : MonoBehaviour, ISaveable
{
    //Used in health regeneration to regenerate health on level up to percentage of new level max health
    [SerializeField] float regenerationPercentage = 70f;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] public LazyValue<float> currentHealth { get; private set; }

    public event Action OnTakeDamage;
    public event Action OnDie;

    public bool IsDead => currentHealth.value == 0f;

    private void Awake()
    {
        currentHealth = new LazyValue<float>(GetInitialHealth);
    }

    private float GetInitialHealth()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (currentHealth.value == 0)
        {
            OnDie?.Invoke();
            

        }
        currentHealth.ForceInit();        
    }

    private void OnEnable()
    {
        GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
    }

    private void OnDisable()
    {
        GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth.value;
    }

    public float GetMaxHealth()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    public float GetPercent()
    {
        return 100 * (currentHealth.value / GetComponent<BaseStats>().GetStat(Stat.Health));
    }

    public void DealDamage(GameObject instigator, float damage)
    {
        if (currentHealth.value <= 0) { return; }

        print(gameObject.name + " took damage: " + damage);

        currentHealth.value -= damage;

        if (currentHealth.value < 0)
        {
            currentHealth.value = 0;
        }

        OnTakeDamage?.Invoke();

        if (currentHealth.value == 0)
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
        currentHealth.value = GetComponent<BaseStats>().GetStat(Stat.Health);

        Debug.Log(currentHealth.value);
    }

    public object CaptureState()
    {
        Dictionary<string, float> data = new Dictionary<string, float>();
        data["currentHealth"] = currentHealth.value;
        data["maxHealth"] = GetMaxHealth();
        return data;
    }

    public void RestoreState(object state)
    {
        Dictionary<string, float> data = (Dictionary<string, float>)state;
        currentHealth.value = data["currentHealth"];
        if (currentHealth.value == 0)
        {
            OnDie?.Invoke();
            return;


        }
        maxHealth = data["maxHealth"];

        
    }
}
