using GameDevTV.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameDevTV.Saving;
using Newtonsoft.Json.Linq;

public class Stamina : MonoBehaviour, IJsonSaveable
{
    [SerializeField] float regenerationPercentage = 70f;

    [SerializeField] float percentLossOnBlock;
    [SerializeField] float percentLossOnDamage;
    [SerializeField] float staminaRegenRate;
    [SerializeField] float staminaRegenDelay;

    [SerializeField] float staminaForDisplay;
    [SerializeField] public LazyValue<float> currentStamina { get; private set; }

    private Coroutine staminaRegenCoroutine;

    private void OnEnable()
    {
        GetComponent<Health>().OnTakeDamage += LoseStaminaOnDamage;
        GetComponent<DamageReceiver>().OnBlocked += LoseStaminaBlocked;
        GetComponent<BaseStats>().onLevelUp += RegenerateStamina;
    }

    private void LoseStaminaBlocked()
    {
        currentStamina.value -= (currentStamina.value / percentLossOnBlock);
        RestartStaminaRegen();

    }

    private void OnDisable()
    {
        GetComponent<Health>().OnTakeDamage -= LoseStaminaOnDamage;
        GetComponent<DamageReceiver>().OnBlocked -= LoseStaminaBlocked;
        GetComponent<BaseStats>().onLevelUp -= RegenerateStamina;
    }

    private void Awake()
    {
        currentStamina = new LazyValue<float>(GetInitialStamina);
    }

    private void LoseStaminaOnDamage()
    {
        currentStamina.value -= (currentStamina.value / percentLossOnDamage);
        RestartStaminaRegen();
    }

    private void RestartStaminaRegen()
    {
        if (staminaRegenCoroutine != null)
        {
            StopCoroutine(staminaRegenCoroutine);
        }
        staminaRegenCoroutine = StartCoroutine(RegenerateStaminaOverTime());
    }

    private IEnumerator RegenerateStaminaOverTime()
    {
        yield return new WaitForSeconds(staminaRegenDelay); // Delay before starting regeneration

        while (currentStamina.value < GetMaxStamina())
        {
            currentStamina.value += GetMaxStamina() * staminaRegenRate * Time.deltaTime; // Adjust regen rate here
            currentStamina.value = Mathf.Min(currentStamina.value, GetMaxStamina()); // Cap at max stamina
            yield return null;
        }
        staminaRegenCoroutine = null;
    }

    private float GetInitialStamina()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Stamina);
    }

    public float GetPercent()
    {
        return 100 * (currentStamina.value / GetComponent<BaseStats>().GetStat(Stat.Stamina));
    }

    public float GetCurrentStamina()
    {
        return currentStamina.value;
    }

    public float GetMaxStamina()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Stamina);
    }

    private void RegenerateStamina()
    {
        //Regen to percentage of the new level max health if below that number
        //float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
        //currentHealth = Mathf.Max(currentHealth, regenHealthPoints);



        //Regenerate to max health
        currentStamina.value = GetComponent<BaseStats>().GetStat(Stat.Stamina);

        Debug.Log(currentStamina.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStamina.ForceInit();
        staminaForDisplay = currentStamina.value;
    }

    // Update is called once per frame
    void Update()
    {
        staminaForDisplay = currentStamina.value;
    }

    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(currentStamina.value);
    }

    public void RestoreFromJToken(JToken state)
    {
        currentStamina.value = state.ToObject<float>();
    }
}
