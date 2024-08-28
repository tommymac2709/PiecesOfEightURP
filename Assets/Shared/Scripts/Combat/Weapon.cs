using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    [SerializeField] GameObject _weaponPrefab = null;

    public void Spawn(Transform handTransform)
    {
        Instantiate(_weaponPrefab, handTransform);
    }
}
