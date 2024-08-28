using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] Weapon weapon = null;
    [SerializeField] Transform _handTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        SpawnWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnWeapon()
    {
        if (weapon == null) return;
        weapon.Spawn(_handTransform);
    }
}
