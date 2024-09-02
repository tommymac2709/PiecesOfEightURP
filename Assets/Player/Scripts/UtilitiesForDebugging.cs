using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilitiesForDebugging : MonoBehaviour
{
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            health.DealDamage(5f);
        }
    }
}
