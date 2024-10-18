using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    LazyValue<SavingWrapper> savingWrapper;

    private void Awake()
    {
        savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
    }

    

    private SavingWrapper GetSavingWrapper()
    {
        return FindObjectOfType<SavingWrapper>();

    }

    public void ContinueGame()
    {
        savingWrapper.value.ContinueGame();
    }
}
