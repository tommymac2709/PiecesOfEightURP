using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    private const string NextPatrolPointIndexKey = "NextPatrolPointIndex";

    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int TargetingSpeedHash = Animator.StringToHash("TargetingRightSpeed");
    private readonly int WalkHash = Animator.StringToHash("Walk");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public EnemyPatrollingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    private float movementSpeed = .5f;    //Default settings
    private float acceptanceRadius = 2f;  //Used if there is not 
    private float dwellTime = 2f;           //A PatrolPoint attached
    private Vector3 targetPatrolPoint;    //To the transform




    public override void Enter()
    {
        //Sanity check.  Ideally this check should never be needed because Patrol State should only
        //be called if there is a PatrolPath to begin with.
        if (stateMachine.PatrolPath == null)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

        int index;
        //Check blackboard for key, set index if key is set.  
        if (stateMachine.Blackboard.ContainsKey(NextPatrolPointIndexKey))
        {
            index = stateMachine.Blackboard.GetValueAsInt(NextPatrolPointIndexKey);
        }
        else
        {
            //The First time we enter a Patrol state, the index will not be set in the Blackboard
            //So se get it from the PatrolPath's GetNearestIndex.
            //We will also be resetting the index if the enemy goes into EnemyChasingState
            index = stateMachine.PatrolPath.GetNearestIndex(stateMachine.transform.position);
        }
        //Set our goal
        targetPatrolPoint = stateMachine.PatrolPath.GetWaypoint(index);
        PatrolPoint patrolPoint = stateMachine.PatrolPath.GetPatrolPoint(index);
        if (patrolPoint) //If the current index has a PatrolPoint attached, then we can adjust the settings from the defaults
        {
            movementSpeed = stateMachine.MovementSpeed * patrolPoint.SpeedModifier;
            acceptanceRadius = patrolPoint.AcceptanceRadius;
            dwellTime = patrolPoint.DwellTime;
        }
        else //If not, then we need to force the calculation of movementSpeed to be that percentage of the statemachine's movement speed.
        {
            movementSpeed *= stateMachine.MovementSpeed;
        }
        //Squaring the acceptanceRadius now to save some calculation time when we use sqrMagnitude.
        acceptanceRadius *= acceptanceRadius;
        //Setup our next waypoint index
        stateMachine.Blackboard[NextPatrolPointIndexKey] = stateMachine.PatrolPath.GetNextIndex(index);
        //Since the waypoint won't move, we can simply set the destination here on the Agent
        stateMachine.Agent.SetDestination(targetPatrolPoint);
        //Set the animation
        stateMachine.Animator.CrossFadeInFixedTime(WalkHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (deltaTime == 0f) { return; }
        if (IsInChaseRange() && ShouldBeHostile())
        {
            //Clearing key to ensure that at the end of the battle, the enemy finds the nearest waypoint
            stateMachine.Blackboard.Remove(NextPatrolPointIndexKey);
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        if (IsInAcceptanceRange())
        {
            //Once we're close enough to the waypoint, we head to a Dwell state
            stateMachine.SwitchState(new EnemyDwellState(stateMachine, dwellTime));
            return;
        }
        //This code is the same as our Chase State
        Vector3 lastPosition = stateMachine.transform.position;
        MoveToWayPoint(deltaTime);
        Vector3 deltaMovement = lastPosition - stateMachine.transform.position;
        float deltaMagnitude = deltaMovement.magnitude;
        float grossSpeed = deltaMagnitude / deltaTime;
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, grossSpeed / stateMachine.MovementSpeed, AnimatorDampTime, deltaTime);
        if (deltaMagnitude > 0)
        {
            FaceTarget(stateMachine.transform.position - deltaMovement, deltaTime);
        }
        else
        {
            FaceTarget(targetPatrolPoint, deltaTime);
        }
    }

    private bool IsInAcceptanceRange()
    {
        return (stateMachine.transform.position - targetPatrolPoint).sqrMagnitude < acceptanceRadius;
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    void MoveToWayPoint(float deltaTime)
    {
        Vector3 direction = stateMachine.Agent.desiredVelocity.normalized;
        Move(direction * movementSpeed, deltaTime);
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        stateMachine.Agent.nextPosition = stateMachine.transform.position;
    }
}

