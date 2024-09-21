using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IModifierProvider
{
    [SerializeField] WeaponConfig unarmedWeaponConfig = null;
    [SerializeField] WeaponConfig defaultWeaponConfig = null;
    [SerializeField] Transform _rightHandTransform = null;
    [SerializeField] Transform _leftHandTransform = null;

    public WeaponConfig currentWeaponConfig = null;
    public WeaponConfig currentlyUsingWeaponConfig = null;
    Weapon currentWeapon;
    public AttackData currentAttack;
    private Coroutine hitCoroutine = null;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    public bool isHoldingWeapon {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(defaultWeaponConfig);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeaponConfig == unarmedWeaponConfig)
        {
            isHoldingWeapon = false;
        }
        else
        {
            isHoldingWeapon = true;
        }

        
    }

    public WeaponConfig GetUnarmedWeapon()
    {
        return unarmedWeaponConfig;
    }

    public WeaponConfig GetCurrentlyUsingWeapon()
    {
        return currentlyUsingWeaponConfig;
    }

    public void EquipWeapon(WeaponConfig weapon)
    {
        currentWeaponConfig = weapon;

        // Assuming Spawn() returns the instantiated weapon's GameObject
        GameObject weaponObject = weapon.Spawn(_rightHandTransform, _leftHandTransform);

        if (weaponObject != null)
        {
            currentWeapon = weaponObject.GetComponent<Weapon>();
        }
        // Get the Weapon component from the spawned weapon

        if (currentWeaponConfig == unarmedWeaponConfig) return;

        currentlyUsingWeaponConfig = weapon;

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

    public IEnumerable<float> GetAdditiveModifiers(Stat stat)
    {
        if (stat == Stat.Damage)
        {
            yield return currentWeaponConfig.GetWeaponDamage();
            //Can have 2 yield returns for multiple weapons 
        }
    }

    public IEnumerable<float> GetPercentageModifiers(Stat stat)
    {
        if (stat == Stat.Damage)
        {
            yield return currentWeaponConfig.GetPercentageBonus();
            //Can have 2 yield returns for multiple weapons 
        }
    }

    public void TryHitStart(int slot)
    {
        
        // Start hit detection with an infinite duration
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }
        hitCoroutine = StartCoroutine(TryHitCoroutine(slot, Mathf.Infinity));
    }

    public void TryHitEnd()
    {
        // Stop hit detection
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
            hitCoroutine = null;
        }
    }

    private IEnumerator TryHitCoroutine(int slot, float duration)
    {
        alreadyCollidedWith.Clear();
        //foreach (var item in alreadyCollidedWith)
        //{
        //    Debug.Log(item.name);
        //}
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            PerformHitDetection(slot);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void PerformHitDetection(int slot)
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

        //Debug.Log($"Attacking with slot {slot}, position {transformPoint}");

        foreach (Collider other in Physics.OverlapSphere(transformPoint, damageRadius))
        {
            if (other.gameObject == gameObject) continue;

            if (alreadyCollidedWith.Contains(other)) { return; }

            

            if (other.TryGetComponent<DamageReceiver>(out DamageReceiver damageReceiver) && other.tag != this.tag)
            {
                alreadyCollidedWith.Add(other);
                //foreach (var item in alreadyCollidedWith)
                //{
                //    Debug.Log("Added " + item.name + " to already hit list");
                //}
                float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
                float damageToDeal = 5f;
                damageReceiver.DealDamage( gameObject, other.transform, damage);
                TryApplyKnockback(other, transform.position);
               
            }
        }
    }

    private void TryApplyKnockback(Collider other, Vector3 position)
    {
        other.GetComponent<ForceReceiver>().AddForce((other.transform.position - position).normalized * currentAttack.Knockback);
    }



}
