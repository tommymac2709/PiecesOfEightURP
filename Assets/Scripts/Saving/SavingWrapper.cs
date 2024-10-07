using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    PlayerStateMachine stateMachine;
    SavingSystem savingSystem;
    [SerializeField] float fadeInTime = 2f;

    const string defaultSaveFile = "save";

    private void Awake()
    {
        stateMachine = GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
    }

    IEnumerator Start()
    {
        savingSystem = GetComponent<SavingSystem>();
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediate();
        yield return savingSystem.LoadLastScene(defaultSaveFile);
        yield return fader.FadeIn(fadeInTime);

        

    }

    private void OnEnable()
    {
        stateMachine.InputReader.SaveGameEvent += SaveGame;
        stateMachine.InputReader.LoadGameEvent += LoadGame;
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
        stateMachine.InputReader.SaveGameEvent -= SaveGame;
        stateMachine.InputReader.LoadGameEvent -= LoadGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
