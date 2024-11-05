using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockImpactState : EnemyBaseState
{
    private readonly int BlockAnimHash = Animator.StringToHash("BlockImpact");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;
    public EnemyBlockImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.SetStateShouldBlock(false);
        stateMachine.Animator.CrossFadeInFixedTime(BlockAnimHash, CrossFadeDuration);
        stateMachine.DamageReceiver.SetIsInvulnerable(true);
    }

    public override void Exit()
    {
        stateMachine.SetStateShouldBlock(true);
        stateMachine.DamageReceiver.SetIsInvulnerable(false);
    }

    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);
        float normalizedTimeAttack = GetNormalizedTime(stateMachine.PlayerStateMachine.Animator, "Attack");
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Impact");
        //if (normalizedTime > 0.75f)
        //{
        //    int rnd = Random.Range(0, 2);
        //    if (rnd == 0)
        //    {
        //        stateMachine.SwitchState(new EnemyAttackState(stateMachine));
        //        return;
        //    }
        //}
        /*else */if (normalizedTime > 0.5f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
