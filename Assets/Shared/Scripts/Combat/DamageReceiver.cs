using System;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public event Action OnParried;

    public event Action OnBlocked;

    [SerializeField] Health health;
    [SerializeField] Stamina stamina;

    //[SerializeField] WeaponHandler weapon;

    [SerializeField] float coveredAngle = 90f;

    private bool isBlocking = false;

    private bool isInvulnerable;

    private bool canParry;


    private void Update()
    {
        
    }

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



    public void DealDamage(GameObject instigator, Transform attacker, float damageAmount)
    {
        instigator.TryGetComponent<EnemyStateMachine>(out var enemyStateMachine);

        //if (enemyStateMachine != null)
        //{
            

            if (canParry && AttackerInCoverage(instigator))
            {
                //weapon.HitOne();
                enemyStateMachine.SwitchState(new EnemyParriedState(enemyStateMachine, enemyStateMachine.ParriedStateDuration));
                return;
            }
        //}
       
        

        if (isBlocking && AttackerInCoverage(instigator) && stamina.GetCurrentStamina() > 0)
        {
            OnBlocked?.Invoke();
            //weapon.HitTwo();
            return;
        }

        if (isInvulnerable) { return; }

        //weapon.HitOne();
        health.DealDamage(instigator, damageAmount);
    }

    private bool AttackerInCoverage(GameObject other)
    {
        var requiredValue = Mathf.Cos(coveredAngle * Mathf.Deg2Rad);
        var directionToAttacker = (other.transform.position - transform.position).normalized;
        var dotProduct = Vector3.Dot(transform.forward, directionToAttacker);
        return dotProduct >= requiredValue;
    }

   
}
