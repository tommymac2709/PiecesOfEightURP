using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingWrapper : MonoBehaviour
{
    
    [SerializeField] float fadeInTime = 2f;

    const string defaultSaveFile = "save";

    public void ContinueGame()
    {
        StartCoroutine(LoadLastScene());
    }

    private IEnumerator LoadLastScene()
    {
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeInTime);
        yield return GetComponent<JSonSavingSystem>().LoadLastScene(defaultSaveFile);
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
        GetComponent<JSonSavingSystem>().Load(defaultSaveFile);
    }

        public void SaveGame()
        {
            GetComponent<JSonSavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<JSonSavingSystem>().Delete(defaultSaveFile);
        }
}

