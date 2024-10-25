using GameDevTV.Saving;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Notoriety : MonoBehaviour/*, IJsonSaveable*/
{
    [SerializeField] float currentPirateNotoriety;
    [SerializeField] float currentNavyNotoriety;
    [SerializeField] float currentCommunityNotoriety;

    [SerializeField] float pirateHosilityThreshold;
    [SerializeField] float navyHosilityThreshold;
    [SerializeField] float communityHosilityThreshold;



    public event Action OnPirateNotorietyIncreased;
    public event Action OnPirateNotorietyDecreased;

    public event Action OnNavyNotorietyIncreased;
    public event Action OnNavyNotorietyDecreased;

    public event Action OnCommunityNotorietyIncreased;
    public event Action OnCommunityNotorietyDecreased;

    SavingWrapper wrapper;

    // Start is called before the first frame update
    void Start()
    {
        wrapper = FindObjectOfType<SavingWrapper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseNotoriety(Faction faction, float amount)
    {
        switch(faction)
        {
            case Faction.Pirate:
                IncreasePirateNotoriety(amount);
                break;

            case Faction.Navy:
                IncreaseNavyNotoriety(amount); 
                break;

            case Faction.Community:
                IncreaseCommunityNotoriety(amount);
                break;
        }
    }

    public void DecreaseNotoriety(Faction faction, float amount)
    {
        switch (faction)
        {
            case Faction.Pirate:
                DecreasePirateNotoriety(amount);
                break;

            case Faction.Navy:
                DecreaseNavyNotoriety(amount);
                break;

            case Faction.Community:
                DecreaseCommunityNotoriety(amount);
                break;
        }
    }


    public void IncreasePirateNotoriety(float amount)
    {
        DecreaseNavyNotoriety(amount);
        DecreaseCommunityNotoriety(amount);
        currentPirateNotoriety += amount;
        wrapper.SaveGame();
        OnPirateNotorietyIncreased?.Invoke();
    }

    public void DecreasePirateNotoriety(float amount)
    {
        currentPirateNotoriety -= amount;
        wrapper.SaveGame();
        OnPirateNotorietyDecreased?.Invoke();
    }

    public void IncreaseNavyNotoriety(float amount)
    {
        DecreasePirateNotoriety(amount);
        IncreaseCommunityNotoriety(amount);
        currentNavyNotoriety += amount;
        wrapper.SaveGame();
        OnNavyNotorietyIncreased?.Invoke();
    }

    public void DecreaseNavyNotoriety(float amount)
    {
        currentNavyNotoriety -= amount;
        wrapper.SaveGame();
        OnNavyNotorietyDecreased?.Invoke();
    }

    public void IncreaseCommunityNotoriety(float amount)
    {
        IncreaseNavyNotoriety(amount);
        currentCommunityNotoriety += amount;
        wrapper.SaveGame();
        OnCommunityNotorietyIncreased?.Invoke();
    }

    public void DecreaseCommunityNotoriety(float amount)
    {
        currentCommunityNotoriety -= amount;
        wrapper.SaveGame();
        OnCommunityNotorietyDecreased?.Invoke();
    }

    public float GetAffiliatedNotoriety(Faction faction)
    {
        float affiliatedNotoriety = 0;

        switch (faction)
        {
            case Faction.Pirate:
                affiliatedNotoriety = GetPirateNotoriety();
                break;

            case Faction.Navy:
                affiliatedNotoriety = GetNavyNotoriety();
                break;

            case Faction.Community:
                affiliatedNotoriety = GetCommunityNotoriety();
                break;
        }
        return affiliatedNotoriety;
    }

    public float GetAffiliatedNotorietyThreshold(Faction faction)
    {
        float affiliatedNotorietyThreshold = 0;
        switch (faction)
        {
            case Faction.Pirate:
                
                affiliatedNotorietyThreshold = GetPirateHostilityThreshold();
                break;

            case Faction.Navy:
                
                affiliatedNotorietyThreshold = GetNavyHostilityThreshold();
                break;

            case Faction.Community:
                
                affiliatedNotorietyThreshold = GetCommunityHostilityThreshold();
                break;
        }
        return affiliatedNotorietyThreshold;
    }

    public float GetPirateHostilityThreshold()
    {
        return pirateHosilityThreshold;
    }

    public float GetNavyHostilityThreshold()
    {
        return navyHosilityThreshold;
    }

    public float GetCommunityHostilityThreshold()
    {
        return communityHosilityThreshold;
    }

    public float GetPirateNotoriety()
    {
        return currentPirateNotoriety;
    }

    public float GetNavyNotoriety()
    {
        return currentNavyNotoriety;
    }

    public float GetCommunityNotoriety()
    {
        return currentCommunityNotoriety;
    }

    //public JToken CaptureAsJToken()
    //{
    //    throw new NotImplementedException();
    //}

    //public void RestoreFromJToken(JToken state)
    //{
    //    throw new NotImplementedException();
    //}
}
