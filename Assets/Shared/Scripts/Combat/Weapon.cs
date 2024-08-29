using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    [SerializeField] GameObject _weaponPrefab = null;
    [SerializeField] float _weaponDamage;
    [SerializeField] bool isRightHanded = true;

    [field: SerializeField] public AttackData[] Attacks { get; private set; } = null;


    const string weaponName = "Weapon";
    

    public void Spawn(Transform rightHandTransform, Transform leftHandTransform)
    {
        DestroyOldWeapon(rightHandTransform, leftHandTransform);

        if (_weaponPrefab != null)
        {
            Transform handTransform = GetTransform(rightHandTransform, leftHandTransform);
            GameObject weapon = Instantiate(_weaponPrefab, handTransform);
            weapon.name = weaponName;
        }
        
    }

    private Transform GetTransform(Transform rightHandTransform, Transform leftHandTransform)
    {
        Transform handTransform;

        if (isRightHanded)
        {
            handTransform = rightHandTransform;
        }
        else
        {
            handTransform = leftHandTransform;
        }
        return handTransform;   
    }

    private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
        Transform oldWeapon = rightHand.Find(weaponName);
        if (oldWeapon == null) 
        { 
            oldWeapon = leftHand.Find(weaponName);
        }
        if (oldWeapon == null) return;

        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject);
    }

    public float GetWeaponDamage()
    {
        return _weaponDamage;
    }
}
