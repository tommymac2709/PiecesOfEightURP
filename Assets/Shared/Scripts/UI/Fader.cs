using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

       
    }

    public void FadeOutImmediate()
    {
        canvasGroup.alpha = 1.0f;
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 0.95)
        {
            canvasGroup.alpha += Time.unscaledDeltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime / time;
            yield return null;
        }
    }

}
