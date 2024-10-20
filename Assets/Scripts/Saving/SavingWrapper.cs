using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingWrapper : MonoBehaviour
{
    
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] int firstSceneBuildIndex = 1;
    [SerializeField] int menuSceneBuildIndex = 0;

    private void Start()
    {
        
    }

    private const string currentSaveKey = "currentSaveName";

    public void ContinueGame()
    {
        if (!PlayerPrefs.HasKey(currentSaveKey)) { return; }
        if (!GetComponent<JSonSavingSystem>().SaveFileExists(GetCurrentSave())) return;
        StartCoroutine(LoadLastScene());
    }

    public void NewGame(string saveFile)
    {
        if (String.IsNullOrEmpty(saveFile)) { return; }
        SetCurrentSave(saveFile);
        StartCoroutine(LoadFirstScene());
    }

    private void SetCurrentSave(string saveFile)
    {
        PlayerPrefs.SetString(currentSaveKey, saveFile);
    }

    public void LoadGame(string saveFile)
    {
        SetCurrentSave(saveFile);
        ContinueGame();
    }

    public string GetCurrentSave()
    {
        return PlayerPrefs.GetString(currentSaveKey);
    }
    private IEnumerator LoadFirstScene()
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeInTime);
        yield return SceneManager.LoadSceneAsync(firstSceneBuildIndex);
        yield return fader.FadeIn(fadeInTime);
        SaveGame();


    }

    private IEnumerator LoadMenuScene()
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeInTime);
        yield return SceneManager.LoadSceneAsync(menuSceneBuildIndex);
        yield return fader.FadeIn(fadeInTime);
    }

    private IEnumerator LoadLastScene()
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeInTime);
        yield return GetComponent<JSonSavingSystem>().LoadLastScene(GetCurrentSave());
        yield return fader.FadeIn(fadeInTime);

    }


    public void LoadGame()
    {
        GetComponent<JSonSavingSystem>().Load(GetCurrentSave());
    }

        public void SaveGame()
        {
            GetComponent<JSonSavingSystem>().Save(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<JSonSavingSystem>().Delete(GetCurrentSave());
        }

    public IEnumerable<string> ListSaves()
    {
        return GetComponent<JSonSavingSystem>().ListSaves();
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadMenuScene());
    }
}

