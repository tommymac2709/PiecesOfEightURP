using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] Weapon defaultWeapon = null;
    [SerializeField] Transform _rightHandTransform = null;
    [SerializeField] Transform _leftHandTransform = null;

    Weapon _currentWeapon = null;

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
}
