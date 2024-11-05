using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockingState : EnemyBaseState
{
    private float blockingDuration = 0.25f; // Time in seconds
    private float timer = 0;

    private readonly int blockAnimHash;
    

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    public EnemyBlockingState(EnemyStateMachine stateMachine, AttackData playerAttackData) : base(stateMachine) 
    {
        blockAnimHash = Animator.StringToHash(playerAttackData.BlockAnimationName);
    }

    public override void Enter()
    {
        PlayerAttackingState.OnPlayerAttackComplete += StopBlocking;
        stateMachine.Animator.CrossFadeInFixedTime(blockAnimHash, CrossFadeDuration);
        stateMachine.DamageReceiver.SetIsBlocking(true); // Set enemy blocking property
    }

    public override void Tick(float deltaTime)
    {
        //float normalizedTime = GetNormalizedTime(stateMachine.PlayerStateMachine.Animator, "Attack");
        //if (normalizedTime > 0.5f) 
        //{
        //    int rnd = Random.Range(0, 2);
        //    if (rnd == 0)
        //    {
        //        stateMachine.SwitchState(new EnemyAttackState(stateMachine));
        //        return;
        //    }
        //}
    }

    public override void Exit()
    {
        PlayerAttackingState.OnPlayerAttackComplete -= StopBlocking;
        stateMachine.DamageReceiver.SetIsBlocking(false); // Reset blocking property
        
    }

    private void StopBlocking()
    {
        int rnd = Random.Range(0, 2);
        if (rnd == 0)
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine));
            return;
        }
        else if (rnd == 1)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine)); // Switch out of blocking
        }
        
    }
    private bool PlayerOutOfRange()
    {
        return Vector3.Distance(stateMachine.transform.position, stateMachine.Player.transform.position) > stateMachine.BlockRange;
    }
}

