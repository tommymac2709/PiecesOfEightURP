using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] WeaponConfig defaultWeaponConfig = null;
    [SerializeField] Transform _rightHandTransform = null;
    [SerializeField] Transform _leftHandTransform = null;

    public WeaponConfig currentWeaponConfig = null;
    Weapon currentWeapon;
    public AttackData currentAttack;
    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(defaultWeaponConfig);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
        currentWeaponConfig = weapon;

        // Assuming Spawn() returns the instantiated weapon's GameObject
        GameObject weaponObject = weapon.Spawn(_rightHandTransform, _leftHandTransform);

        // Get the Weapon component from the spawned weapon
        currentWeapon = weaponObject.GetComponent<Weapon>();
    }


    /// <summary>
    /// Get specified animation by index, and set the current Attack.
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public AttackData GetCurrentAttack(int attack)
    {
        if (currentWeaponConfig == null || currentWeaponConfig.Attacks.Length == 0) return null;
        if (attack < 0 || attack >= currentWeaponConfig.Attacks.Length)
        {
            attack = 0;
        }
        currentAttack = currentWeaponConfig.Attacks[attack];
        return currentAttack;
    }

    /// <summary>
    /// Sets the current attack.  Useful when attack contains a combo attack follow on.
    /// </summary>
    /// <param name="attack"></param>
    public void SetCurrentAttack(AttackData attack)
    {
        currentAttack = attack;
    }

    public void TryHit(int slot)
    {
        if (currentAttack == null) return;
        Vector3 transformPoint;
        float damageRadius = .5f;
        switch (slot)
        {
            case 0:
                transformPoint = currentWeapon.DamagePoint;
                damageRadius = currentWeapon.DamageRadius;
                break;
            case 1:
                transformPoint = _rightHandTransform.position;
                break;
            case 2:
                transformPoint = _leftHandTransform.position;
                break;
            default:
                transformPoint = _rightHandTransform.position;
                break;
        }
        Debug.Log($"Attacking with slot {slot}, position {transformPoint}");
        foreach (Collider other in Physics.OverlapSphere(transformPoint, damageRadius))
        {
            if (other.gameObject == gameObject) continue;
            
            if (other.TryGetComponent<DamageReceiver>(out DamageReceiver damageReceiver))
            {
                damageReceiver.DealDamage(other.transform, 1);
            }
        }
    }



}
