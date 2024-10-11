using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    PlayerStateMachine stateMachine;
    JSonSavingSystem savingSystem;
    [SerializeField] float fadeInTime = 2f;

    const string defaultSaveFile = "save";

    private void Awake()
    {
        stateMachine = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
    }

    IEnumerator Start()
    {
        savingSystem = GetComponent<JSonSavingSystem>();
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediate();
        yield return savingSystem.LoadLastScene(defaultSaveFile);
        yield return fader.FadeIn(fadeInTime);

        

    }

    private void OnEnable()
    {
        InputReader.SaveGameEvent += SaveGame;
        InputReader.LoadGameEvent += LoadGame;
    }

    public void LoadGame()
    {
        
        savingSystem.Load(defaultSaveFile);
    }

    public void SaveGame()
    {
        savingSystem.Save(defaultSaveFile);
    }

    private void OnDisable()
    {
        InputReader.SaveGameEvent -= SaveGame;
        InputReader.LoadGameEvent -= LoadGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
