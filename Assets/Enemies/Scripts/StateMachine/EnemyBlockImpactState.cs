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
        stateMachine.Animator.CrossFadeInFixedTime(BlockAnimHash, CrossFadeDuration);
        stateMachine.DamageReceiver.SetIsInvulnerable(true);
    }

    public override void Exit()
    {
        stateMachine.DamageReceiver.SetIsInvulnerable(false);
    }

    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Impact");
        if (normalizedTime > 0.9f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
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
