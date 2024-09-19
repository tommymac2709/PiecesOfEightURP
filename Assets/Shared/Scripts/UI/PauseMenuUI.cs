using System;
using System.Linq;
using UnityEngine;


public class PauseMenuUI : WindowController
{

    private void Start()
    {
        CloseWindow();
    }

    protected override void Subscribe()
    {
        GameObject.FindWithTag("Player").GetComponent<InputReader>().CancelWindowEvent += HandleCancelEvent;
    }

    protected override void Unsubscribe()
    {
        GameObject.FindWithTag("Player").GetComponent<InputReader>().CancelWindowEvent -= HandleCancelEvent;
    }

    private void HandleCancelEvent()
    {
        if (activeWindows.Count > 0)
        {
            var windows = activeWindows.ToList();
            for (int i = 0; i < activeWindows.Count; i++)
            {
                windows[i].gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    //public void Save()
    //{
    //    SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
    //    savingWrapper.Save();
    //}

    //public void SaveAndQuit()
    //{
    //    SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
    //    savingWrapper.Save();
    //    savingWrapper.LoadMenu();
    //}
}

