using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowController : MonoBehaviour
{
    private static HashSet<WindowController> activeWindows = new();

    protected InputReader InputReader;
    protected virtual void Awake()
    {
        InputReader = GameObject.FindWithTag("Player").GetComponent<InputReader>();
        Subscribe();
    }

    protected virtual void Destroy()
    {
        Unsubscribe();
    }

    void OnEnable()
    {
        activeWindows.Add(this);
        Time.timeScale = 0.0f;
    }

    void OnDisable()
    {
        activeWindows.Remove(this);
        if (activeWindows.Count == 0) Time.timeScale = 1.0f;
    }

    /// <summary>
    /// Override this method to subscribe to any events that will call this implementation of WindowController.
    /// </summary>
    protected abstract void Subscribe();

    /// <summary>
    /// Override this method to unsubscribe to any events that will call this implementation of WindowController.
    /// </summary>
    protected abstract void Unsubscribe();

    protected void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    protected void ToggleWindow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}

