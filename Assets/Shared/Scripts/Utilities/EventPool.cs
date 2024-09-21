using System;
using System.Collections.Generic;

public class EventPool
{
    /// <summary>
    /// Represents a variable pool that stores actions associated with string keys. </summary>
    /// /
    private Dictionary<string, Action> pool = new Dictionary<string, Action>();

    /// <summary>
    /// Adds a listener to the specified token. If the token already exists in the pool, the listener will be appended to the existing token's Action event.
    /// </summary>
    /// <param name="token">The token to add the listener to.</param>
    /// <param name="action">The listener action to be added.</param>
    public void AddListener(string token, Action action)
    {
        if (!pool.TryAdd(token, action))
            pool[token] += action;
    }


    /// <summary>
    /// Removes a listener from the pool of actions associated with a given token.
    /// </summary>
    /// <param name="token">The token associated with the listener being removed.</param>
    /// <param name="action">The action to remove from the pool.</param>
    public void RemoveListener(string token, Action action)
    {
        if (pool.ContainsKey(token)) pool[token] -= action;
    }

    /// <summary>
    /// Invokes an event associated with the specified token.
    /// </summary>
    /// <param name="token">The token associated with the event.</param>
    public void InvokeEvent(string token)
    {
        if (pool.ContainsKey(token)) pool[token]?.Invoke();
    }
}

