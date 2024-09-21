using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    [field: SerializeField] public float SpeedModifier { get; private set; } = .5f;
    [field: SerializeField] public float AcceptanceRadius { get; private set; } = 2.0f;
    [field: SerializeField] public float DwellTime { get; private set; } = 3.0f;
}

