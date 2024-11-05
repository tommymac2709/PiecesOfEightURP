using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBlockedImpactState : PlayerBaseState
{
    private readonly int BlockAnimHash = Animator.StringToHash("BlockImpact");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.2f;
    public PlayerAttackBlockedImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BlockAnimHash, CrossFadeDuration);
        
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick(float deltaTime)
    {
        MoveNoInput(deltaTime);
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Impact");
        if (normalizedTime > 0.9f) 
        {
            ReturnToLocomotion();
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
