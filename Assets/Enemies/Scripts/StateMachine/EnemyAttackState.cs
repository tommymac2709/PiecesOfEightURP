
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
        stateMachine.SetStateShouldBlock(false);
        if (!attack)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        stateMachine.SetAttacking(true); // Set the enemy as attacking

        level = stateMachine.BaseStats.GetLevel();
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, 0.1f);
    }

    public override void Tick(float deltaTime)
    {
        FaceTarget(stateMachine.Player.transform.position, deltaTime);
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
            stateMachine.SwitchState(new EnemyPreAttackState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.SetStateShouldBlock(true);
        stateMachine.SetAttacking(false); // Set the enemy as not attacking
        stateMachine.CooldownTokenManager.SetCooldown("Attack", attack.Cooldown);
    }
}


