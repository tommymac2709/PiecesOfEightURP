using System;
using UnityEngine;
using GameDevTV.Utils;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using GameDevTV.Saving;
using UnityEngine.Events;

public class Health : MonoBehaviour, IJsonSaveable
{
    //Used in health regeneration to regenerate health on level up to percentage of new level max health
    [SerializeField] private float regenerationPercentage = 70f;
    //For enemy behaviour
    [SerializeField] private float percentHealthToTryFlee = 50f;
    [SerializeField] public LazyValue<float> currentHealth { get; private set; }
    public bool IsDead => currentHealth.value == 0f;

    public event Action OnTakeDamage;
    public event Action OnDie;
    public static event Action OnDeathUI;

    public UnityEvent onResurrection;

    private void OnEnable()
    {
        GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
    }
    private void OnDisable()
    {
        GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
    }
    private void Awake()
    {
        currentHealth = new LazyValue<float>(GetInitialHealth);
    }
    void Start()
    {
        if (currentHealth.value == 0)
        {
            OnDie?.Invoke();
        }

        currentHealth.ForceInit();
    }

    private float GetInitialHealth()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Health);
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

    private void RegenerateHealth()
    {
        //Regen to percentage of the new level max health if below that number
        //float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
        //currentHealth = Mathf.Max(currentHealth, regenHealthPoints);

        //Regenerate to max health
        currentHealth.value = GetComponent<BaseStats>().GetStat(Stat.Health);

        Debug.Log(currentHealth.value);
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

            if (gameObject.CompareTag("Player"))
            {
                OnDeathUI?.Invoke();
            }
            
            AwardExperience(instigator);
        }

        Debug.Log(currentHealth);

        //the above from currentHealth-= damage can be written as:
        //currentHealth = Mathf.Max(currentHealth - damage, 0);
    }

    private void AwardExperience(GameObject instigator)
    {
        Experience experience = instigator.GetComponent<Experience>();
        Notoriety notoriety = instigator.GetComponent<Notoriety>();
        Debug.Log("Attacker was " + instigator.gameObject.name);

        if (experience == null) { return; }
        if (notoriety == null) { return; }

        experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        notoriety.IncreaseNotoriety(GetComponent<BaseStats>().GetFaction(), 10f);
    }

    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(currentHealth.value);
    }

    public void RestoreFromJToken(JToken state)
    {
        currentHealth.value = state.ToObject<float>();
        
    }

}
