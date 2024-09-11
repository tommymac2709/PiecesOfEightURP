using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindowToggle : WindowController
{
    protected override void Subscribe()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<InputReader>().InventoryEvent += ToggleWindow;
        gameObject.SetActive(false);
    }

    protected override void Unsubscribe()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<InputReader>().InventoryEvent -= ToggleWindow;
    }

}
