using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider playerCollider;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private int damage;

    private float knockback;

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider) { return; }

        if (alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<DamageReceiver>(out DamageReceiver damageReceiver))
        {
            damageReceiver.DealDamage(playerCollider.transform, damage);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - playerCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);

        }
    }

    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
