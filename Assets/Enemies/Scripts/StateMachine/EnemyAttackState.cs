using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private readonly int AttackAnimHash = Animator.StringToHash("Attack");
    
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private float previousFrameTime;
    private AttackData currentAttack;

    private bool hasCombo;

    bool compile;

    

    public EnemyAttackState(EnemyStateMachine stateMachine, AttackData attack) : base(stateMachine)
    {
        currentAttack = attack;
        stateMachine.Fighter.SetCurrentAttack(attack);

    }

    public EnemyAttackState(EnemyStateMachine stateMachine, int attack) : base(stateMachine)
    {
        currentAttack = stateMachine.Fighter.GetCurrentAttack(attack);
    }

    public override void Enter()
    {
        if (currentAttack.ApplyRootMotion) stateMachine.Animator.applyRootMotion = true;
        hasCombo = currentAttack.NextComboAttack != null;

        stateMachine.Animator.CrossFadeInFixedTime(currentAttack.AnimationName, currentAttack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            if (hasCombo)
            {
                ComboAttack(normalizedTime);
            }
        }

        previousFrameTime = normalizedTime;

        FacePlayer();

        if (!IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
    }

    private void ComboAttack(float normalizedTime)
    {
        if (normalizedTime < currentAttack.ComboAttackTime) { return; }

        stateMachine.SwitchState(new EnemyAttackState(stateMachine, currentAttack.NextComboAttack));
    }

    public override void Exit()
    {
        
    }

    
}
