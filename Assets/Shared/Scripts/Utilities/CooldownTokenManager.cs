using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages cooldown tokens and provides methods for setting, retrieving, and clearing cooldowns.
/// </summary>
public class CooldownTokenManager : MonoBehaviour
{

    private class CooldownRecord
    {

        public float TimeRemaining;
        public float Duration;
        public Coroutine Routine;

        /// <summary>
        /// Calculates the percentage of time remaining compared to the total duration.
        /// </summary>
        /// <returns>The percentage of time remaining as a float.</returns>
        public float GetPercentage() => TimeRemaining / Duration;

    }

    /// <summary>
    /// Event that is triggered when a cooldown starts.
    /// </summary>
    public EventPool OnCooldownStarted = new EventPool();

    /// <summary>
    /// Represents an event pool for the OnCooldownTick event.
    /// </summary>
    public EventPool OnCooldownTick = new EventPool();

    /// <summary>
    /// Event pool for when the cooldown is finished.
    /// </summary>
    public EventPool OnCooldownFinished = new EventPool();


    /// <summary>
    /// A private Dictionary variable that stores active cooldowns.
    /// The keys are strings representing the names of the cooldowns, and the values are CooldownRecord objects containing cooldown information.
    /// </summary>
    private Dictionary<string, CooldownRecord> activeCooldowns = new Dictionary<string, CooldownRecord>();

    /// <summary>
    /// Checks if the given token has an active cooldown. If true, token's cooldown is open and any action you intend to take should not be taken.
    /// </summary>
    /// <param name="token">The token to check for a cooldown.</param>
    /// <returns>True if the token has an active cooldown, otherwise false.</returns>
    public bool HasCooldown(string token) => activeCooldowns.ContainsKey(token);

    /// <summary>
    /// Get the remaining time of a cooldown associated with the given token.
    /// </summary>
    /// <param name="token">The token associated with the cooldown.</param>
    /// <returns>The remaining time in seconds. Returns 0 if the token is null, empty, or the cooldown does not exist.</returns>
    public float GetTimeRemaining(string token)
    {
        if (string.IsNullOrEmpty(token)) return 0;
        if (activeCooldowns.ContainsKey(token)) return activeCooldowns[token].TimeRemaining;
        return 0;
    }

    /// <summary>
    /// Retrieves the duration of a cooldown associated with a given token.  This is how long the cooldown was set to run when it began.
    /// </summary>
    /// <param name="token">The token representing the cooldown.</param>
    /// <returns>The duration of the cooldown associated with the given token. Returns 0 if the token is null or empty, or if there is no cooldown associated with the token.</returns>
    public float GetDuration(string token)
    {
        if (string.IsNullOrEmpty(token)) return 0;
        if (activeCooldowns.ContainsKey(token)) return activeCooldowns[token].Duration;
        return 0;
    }

    /// <summary>
    /// Get the percentage value associated with the given token.
    /// </summary>
    /// <param name="token">The token used to retrieve the percentage.</param>
    /// <returns>The percentage value associated with the token. Returns 0 if the token is null or empty, or if no percentage value is found for the token.</returns>
    public float GetPercentage(string token)
    {
        if (string.IsNullOrEmpty(token)) return 0;
        if (activeCooldowns.ContainsKey(token)) return activeCooldowns[token].GetPercentage();
        return 0;
    }


    /// <summary>
    /// Sets the cooldown for a given token.
    /// </summary>
    /// <param name="token">The token representing the cooldown.</param>
    /// <param name="duration">The duration of the cooldown in seconds.</param>
    /// <param name="append">Whether to append the duration to the existing cooldown.</param>
    public void SetCooldown(string token, float duration, bool append = false)
    {
        if (activeCooldowns.ContainsKey(token))
        {
            if (!append)
            {
                activeCooldowns[token].TimeRemaining += duration;
                activeCooldowns[token].Duration += duration;
            }
            else
            {
                activeCooldowns[token].TimeRemaining = duration;
                activeCooldowns[token].Duration = duration;
            }
        }
        else
        {
            var record = new CooldownRecord { TimeRemaining = duration, Duration = duration };
            activeCooldowns.Add(token, record);
            record.Routine = StartCoroutine(CooldownRoutine(token, record));
        }
    }

    /// <summary>
    /// Clears the cooldown for a specific token.
    /// </summary>
    /// <param name="token">The token identifying the cooldown.</param>
    public void ClearCooldown(string token)
    {
        if (activeCooldowns.ContainsKey(token))
        {
            StopCoroutine(activeCooldowns[token].Routine);
            activeCooldowns.Remove(token);
            OnCooldownFinished.InvokeEvent(token);
        }
    }

    /// <summary>
    /// Routine for handling cooldowns.
    /// </summary>
    /// <param name="token">The token associated with the cooldown.</param>
    /// <param name="record">The cooldown record containing the remaining time.</param>
    /// <returns>An enumerator representing the coroutine.</returns>
    private IEnumerator CooldownRoutine(string token, CooldownRecord record)
    {
        OnCooldownStarted.InvokeEvent(token);
        while (record.TimeRemaining > 0)
        {
            float delay = record.TimeRemaining > 1 ? 1 : .1f;
            yield return new WaitForSeconds(delay);
            record.TimeRemaining -= delay;
            OnCooldownTick.InvokeEvent(token);

        }
        activeCooldowns.Remove(token);
        OnCooldownFinished.InvokeEvent(token);
    }
}


