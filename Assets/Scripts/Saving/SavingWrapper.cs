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

    private string GetCurrentSave()
    {
        return PlayerPrefs.GetString(currentSaveKey);
    }
    private IEnumerator LoadFirstScene()
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeInTime);
        yield return SceneManager.LoadSceneAsync(firstSceneBuildIndex);
        yield return fader.FadeIn(fadeInTime);
    }

    private IEnumerator LoadLastScene()
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeInTime);
        yield return GetComponent<JSonSavingSystem>().LoadLastScene(GetCurrentSave());
        yield return fader.FadeIn(fadeInTime);

    }

     private void Update() 
     {
            if (Input.GetKeyDown(KeyCode.O))
            {
                SaveGame();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadGame();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
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
}

