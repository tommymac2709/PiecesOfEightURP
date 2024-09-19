using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowController : MonoBehaviour
{
    protected static HashSet<WindowController> activeWindows = new();

    public static event System.Action OnAnyWindowOpened;
    public static event System.Action OnAllWindowsClosed;

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
        OnAnyWindowOpened?.Invoke();
    }

    void OnDisable()
    {
        activeWindows.Remove(this);
        if (activeWindows.Count == 0)
        {
            Time.timeScale = 1.0f;
            OnAllWindowsClosed?.Invoke();
        }
    }

    /// <summary>
    /// Override this method to subscribe to any events that will call this implementation of WindowController.
    /// </summary>
    protected abstract void Subscribe();

    /// <summary>
    /// Override this method to unsubscribe to any events that will call this implementation of WindowController.
    /// </summary>
    protected abstract void Unsubscribe();

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    protected void ToggleWindow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}


