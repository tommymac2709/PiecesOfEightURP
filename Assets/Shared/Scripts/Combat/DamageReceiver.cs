using System;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public event Action OnParried;

    [SerializeField] Health health;

    [SerializeField] WeaponHandler weapon;

    [SerializeField] float coveredAngle = 90f;

    private bool isBlocking = false;

    private bool isInvulnerable;

    private bool canParry;




    public void SetIsInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void SetIsBlocking(bool state)
    {
        isBlocking = state;
    }

    public void SetCanParry(bool state)
    {
        canParry = state;
    }



    public void DealDamage(GameObject instigator, Transform attacker, int damageAmount)
    {
        attacker.TryGetComponent<EnemyStateMachine>(out var enemyStateMachine);

        if (enemyStateMachine != null)
        {
            if (canParry && AttackerInCoverage(attacker))
            {
                //weapon.HitOne();
                enemyStateMachine.SwitchState(new EnemyParriedState(enemyStateMachine, 2f));
                return;
            }
        }
       
        if (isInvulnerable) { return; }

        if (isBlocking && AttackerInCoverage(attacker))
        {
            //weapon.HitTwo();
            return;
        }

        //weapon.HitOne();
        health.DealDamage(instigator, damageAmount);
    }

    private bool AttackerInCoverage(Transform other)
    {
        var requiredValue = Mathf.Cos(coveredAngle * Mathf.Deg2Rad);
        var directionToAttacker = (other.position - transform.position).normalized;
        var dotProduct = Vector3.Dot(transform.forward, directionToAttacker);
        return dotProduct >= requiredValue;
    }

   
}
