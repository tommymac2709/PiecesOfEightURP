
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private AttackData attack;
    private int level;
    private bool triedCombo;
   

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
        attack = stateMachine.Fighter.GetCurrentAttack(0);
    }

    public EnemyAttackState(EnemyStateMachine stateMachine, AttackData attack) : base(stateMachine)
    {
        this.attack = attack;
        stateMachine.Fighter.SetCurrentAttack(attack);
    }

    public override void Enter()
    {
        if (!attack)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }

        level = stateMachine.BaseStats.GetLevel();
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");
        if (attack.NextComboAttack && !triedCombo && normalizedTime > attack.ComboAttackTime)
        {
            if (level * 5 > Random.Range(0, 51))
            {
                stateMachine.SwitchState(new EnemyAttackState(stateMachine, attack.NextComboAttack));
                return;
            }
            triedCombo = true;
        }

        if (normalizedTime > .98f)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.CooldownTokenManager.SetCooldown("Attack", attack.Cooldown);
    }
}


