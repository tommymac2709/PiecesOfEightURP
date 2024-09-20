using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private readonly int AttackAnimHash = Animator.StringToHash("Attack");
    
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private float previousFrameTime;
    private AttackData currentAttack;

    private bool triedCombo = false;

    private int level;

    bool compile;



    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        currentAttack = stateMachine.Fighter.GetCurrentAttack(0);
    }

    public EnemyAttackState(EnemyStateMachine stateMachine, AttackData attack) : base(stateMachine)
    {
        this.currentAttack = attack;
        stateMachine.Fighter.SetCurrentAttack(attack);
    }


    public override void Enter()
    {
        if (currentAttack.ApplyRootMotion) stateMachine.Animator.applyRootMotion = true;
        // hasCombo = currentAttack.NextComboAttack != null;
        if (!currentAttack)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

        stateMachine.Animator.CrossFadeInFixedTime(currentAttack.AnimationName, currentAttack.TransitionDuration);

        
    }

    public override void Tick(float deltaTime)
    {
        
        FaceTarget(stateMachine.Player.transform.position, deltaTime);
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (currentAttack.NextComboAttack && !triedCombo  )
        {

            if (normalizedTime >= currentAttack.ComboAttackTime)

            stateMachine.SwitchState(new EnemyAttackState(stateMachine, currentAttack.NextComboAttack));
            Debug.Log("Enemy Combo attack!");
            return;


        }
        triedCombo = true;

        //previousFrameTime = normalizedTime;

        // FaceTarget(stateMachine.Player.transform.position, deltaTime);

        //if (!IsInAttackRange())
        //{

        //    stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        //    return;
        //}
        if (normalizedTime > .98f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
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
