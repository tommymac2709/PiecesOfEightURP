using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// Represents a blackboard that stores key-value pairs of data.
/// </summary>
public class Blackboard
{
    private JObject blackBoard;

    /// <summary>
    /// Use to generate a new Blackboard with all keys reset
    /// </summary>
    public Blackboard()
    {
        blackBoard = new JObject();
    }

    /// <summary>
    /// Used for restoring the Blackboard's from a saved state, for use with RestoreFromJToken
    /// </summary>
    /// <param name="token">state passed to RestoreFromJToken, or state cast to a JToken if using old Saving System</param>
    public Blackboard(JToken token)
    {
        if (token is JObject jObject)
        {
            blackBoard = jObject;
        }
        else
        {
            blackBoard = new JObject();
        }
    }

    /// <summary>
    /// Used for capturing the current blackboard state.
    /// </summary>
    /// <returns>A JToken representing the contents of the Blackboard</returns>
    public JToken GetData() => blackBoard;

    /// <summary>
    /// Represents an indexer that provides access to values in the blackBoard dictionary.
    /// </summary>
    /// <param name="index">The key to access the value in the blackBoard dictionary.</param>
    /// <returns>The value associated with the specified key if it exists in the blackBoard dictionary; otherwise, a new JValue with an empty string.</returns>
    public JToken this[string index]
    {
        get
        {
            return blackBoard.TryGetValue(index, out JToken value) ? value : new JValue("");
        }
        set
        {
            blackBoard[index] = value;
        }
    }

    /// <summary>
    /// Retrieves the value associated with the specified key as an integer from the blackboard. If the key does not exist or the associated value is not convertible to an integer, the fallback
    /// value is returned. </summary>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <param name="fallback">The value to return if the key does not exist or the associated value is not convertible to an integer. Default value is 0.</param>
    /// <returns>The value associated with the specified key as an integer if it exists and is convertible to an integer; otherwise, the fallback value. </returns>
    public int GetValueAsInt(string key, int fallback = 0)
    {
        return blackBoard.TryGetValue(key, out JToken value) ? value.Value<int>() : fallback;
    }

    /// <summary>
    /// Retrieves the value associated with the specified key as a floating-point number.
    /// </summary>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <param name="fallback">The default value to return if the key does not exist or the value cannot be parsed as a float. Default is 0f.</param>
    /// <returns>The value associated with the key if found, otherwise the fallback value.</returns>
    public float GetValueAsFloat(string key, float fallback = 0f)
    {
        return blackBoard.TryGetValue(key, out JToken value) ? value.Value<float>() : fallback;
    }

    /// <summary>
    /// Retrieves the value associated with the specified key as a string.
    /// If the key does not exist, the fallback value is returned.
    /// </summary>
    /// <param name="key">The key to retrieve the value for.</param>
    /// <param name="fallback">The value to return if the key does not exist (default: "")</param>
    /// <returns>The value associated with the specified key as a string, or the fallback value.</returns>
    public string GetValueAsString(string key, string fallback = "")
    {
        return blackBoard.TryGetValue(key, out JToken value) ? value.Value<string>() : fallback;
    }

    /// <summary>
    /// Retrieves the value associated with the specified key as a boolean.
    /// </summary>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <param name="fallback">The value to return if the key is not found.</param>
    /// <returns>
    /// The value associated with the specified key if found, otherwise the fallback value.
    /// </returns>
    public bool GetValueAsBool(string key, bool fallback = false)
    {
        return blackBoard.TryGetValue(key, out JToken value) ? value.Value<bool>() : fallback;
    }

    /// <summary>
    /// Determines whether the blackboard contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate in the blackboard.</param>
    /// <returns>true if the blackboard contains an element with the specified key; otherwise, false.</returns>
    public bool ContainsKey(string key)
    {
        return blackBoard.ContainsKey(key);
    }

    /// <summary>
    /// Removes the specified key from the blackboard.
    /// </summary>
    /// <param name="key">The key to remove from the blackboard.</param>
    public void Remove(string key)
    {
        blackBoard.Remove(key);
    }
}




