using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    LazyValue<SavingWrapper> savingWrapper;

    [SerializeField] TMP_InputField newGameInputField;

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

    public void CreateNewGame()
    {
        savingWrapper.value.NewGame(newGameInputField.text);
    }
}
