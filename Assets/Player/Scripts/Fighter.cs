using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] Weapon defaultWeapon = null;
    [SerializeField] Transform _rightHandTransform = null;
    [SerializeField] Transform _leftHandTransform = null;

    Weapon _currentWeapon = null;

    private Attack currentAttack;

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(defaultWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
       
        weapon.Spawn(_rightHandTransform, _leftHandTransform);
    }

    /// <summary>
    /// Get specified animation by index, and set the current Attack.
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public Attack GetCurrentAttack(int attack)
    {
        if (_currentWeapon == null || _currentWeapon.Attacks.Length == 0) return null;
        if (attack < 0 || attack >= _currentWeapon.Attacks.Length)
        {
            attack = 0;
        }
        currentAttack = _currentWeapon.Attacks[attack];
        return currentAttack;
    }

    /// <summary>
    /// Sets the current attack.  Useful when attack contains a combo attack follow on.
    /// </summary>
    /// <param name="attack"></param>
    public void SetCurrentAttack(Attack attack)
    {
        currentAttack = attack;
    }

    void TryHit(int slot)
    {
        Vector3 transformPoint;
        switch (slot)
        {
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
    }


}
