using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDwellState : EnemyBaseState
{
    private readonly int IdleHash = Animator.StringToHash("Idle");
    
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private const string NextPatrolPointIndexKey = "NextPatrolPointIndex";
    private float dwellTime;


    public EnemyDwellState(EnemyStateMachine stateMachine, float dwellTime) : base(stateMachine)
    {
        this.dwellTime = dwellTime;
    }

    public override void Enter()
    {
        //For now, we're  using the IdleHash (remember we set up an explicit Idle animation for dialogues, etc).
        //An improvement could be to add a state to play to the Patrol Point.  This would let you have some
        //interesting things for the character to do while dwelling... perhaps sitting down, or waving his 
        //weapon around.  The possibilities are endless.  
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);
        if (IsInChaseRange())
        {
            //Clear key so that after leaving chase range we get the nearest waypoint to where we end up
            stateMachine.Blackboard.Remove(NextPatrolPointIndexKey);
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
        dwellTime -= deltaTime;
        if (dwellTime <= 0)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit()
    {

    }
}

