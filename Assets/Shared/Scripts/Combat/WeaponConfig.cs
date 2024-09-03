using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class WeaponConfig : ScriptableObject, IModifierProvider
{
    [SerializeField] public GameObject _weaponPrefab = null;
    [SerializeField] float _weaponDamage;
    [SerializeField] float percentageBonus = 0f;
    [SerializeField] bool isRightHanded = true;

    [field: SerializeField] public AttackData[] Attacks { get; private set; } = null;


    const string weaponName = "Weapon";


    public GameObject Spawn(Transform rightHandTransform, Transform leftHandTransform)
    {
        DestroyOldWeapon(rightHandTransform, leftHandTransform);

        GameObject weapon = null;
        if (_weaponPrefab != null)
        {
            Transform handTransform = GetTransform(rightHandTransform, leftHandTransform);
            weapon = Instantiate(_weaponPrefab, handTransform);
            weapon.name = weaponName;
        }

        return weapon;
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

    public float GetPercentageBonus()
    {
        return percentageBonus;
    }

    public IEnumerable<float> GetAdditiveModifiers(Stat stat)
    {
        if (stat == Stat.Damage)
        {
            yield return _weaponDamage;
        }
    }

    public IEnumerable<float> GetPercentageModifiers(Stat stat)
    {
        if (stat == Stat.Damage)
        {
            yield return percentageBonus;
        }
    }

}
