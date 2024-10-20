using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    // Start is called before the first frame update
  

    public void ReloadGame()
    {
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(0.2f);
        this.gameObject.SetActive(false);
        wrapper.ContinueGame();
    }

    public void LoadMenu()
    {
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.LoadMenu();
    }
}
