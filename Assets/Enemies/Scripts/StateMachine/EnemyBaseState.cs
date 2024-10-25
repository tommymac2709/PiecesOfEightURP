using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void MoveNoInput(float deltaTime)
    {
        //Calls the move function below
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void FaceTarget(Vector3 target, float deltaTime)
    {
        Vector3 directionToTarget = target - stateMachine.transform.position;
        directionToTarget.y = 0;
        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(directionToTarget), stateMachine.RotationSpeed * deltaTime);
    }


    protected void FaceMovementDirection()
    {
        Vector3 lookPos = stateMachine.Agent.destination - stateMachine.transform.position;
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected bool IsInChaseRange()
    {
        if (stateMachine.Player.IsDead) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr  <= stateMachine.PlayerDetectRange * stateMachine.PlayerDetectRange;
    }

    protected bool ShouldBeHostile()
    {
        Notoriety playerNotoriety = GameObject.FindWithTag("Player").GetComponent<Notoriety>();
        float currentAffiliatedNotorietyThreshold = playerNotoriety.GetAffiliatedNotorietyThreshold(stateMachine.BaseStats.GetFaction());
        float currentAffiliatedNotoriety = playerNotoriety.GetAffiliatedNotoriety(stateMachine.BaseStats.GetFaction());
        return currentAffiliatedNotoriety >= currentAffiliatedNotorietyThreshold;
    }

    protected bool IsInAttackRange()
    {
        if (stateMachine.Player.IsDead) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    protected bool IsInFleeRange()
    {
        if (stateMachine.Player.IsDead) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr <= stateMachine.PlayerFleeRange * stateMachine.PlayerFleeRange;
    }

    protected bool IsOtherEnemyAttacking()
    {
        Collider[] hitColliders = Physics.OverlapSphere(stateMachine.transform.position, stateMachine.AttackRange * stateMachine.AttackRange);

        foreach (Collider collider in hitColliders)
        {
            EnemyStateMachine otherEnemy = collider.GetComponent<EnemyStateMachine>();
            if (otherEnemy != null && otherEnemy != stateMachine && otherEnemy.IsAttacking)
            {
                return true; // Another enemy is attacking
            }
        }

        return false; // No other enemies are attacking
    }


    protected bool CanSeePlayer()
    {
        if (stateMachine.PlayerAbilityManager.IsInvisible == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
