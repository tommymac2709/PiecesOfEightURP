using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private bool alreadyAppliedForce = false;

    private float previousFrameTime;

    private AttackData currentAttack;

    private bool hasCombo;

    private float canBlockTime = 0.5f;

    public static event Action OnPlayerAttack;
    public static event Action OnPlayerAttackComplete;

    public PlayerAttackingState(PlayerStateMachine stateMachine, AttackData attack) : base(stateMachine)
    {
        currentAttack = attack;

        stateMachine.Fighter.SetCurrentAttack(attack); //Inform fighter of new attack
        
    }

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attack) : base(stateMachine)
    {
        currentAttack = stateMachine.Fighter.GetCurrentAttack(attack);

    }

    public override void Enter()
    {
        OnPlayerAttack?.Invoke();

        #region(Click to Attack)
        stateMachine.InputReader.AttackPressed += HandleAttackPressed;

        #endregion


        if (currentAttack.ApplyRootMotion) stateMachine.Animator.applyRootMotion = true;
        hasCombo = currentAttack.NextComboAttack != null;


        stateMachine.Animator.CrossFadeInFixedTime(currentAttack.AnimationName, currentAttack.TransitionDuration);
        //stateMachine.WeaponDamage.SetAttack(attackData.DamageAmount, attackData.Knockback);
    }

    public override void Tick(float deltaTime)
    {
        

        MoveNoInput(deltaTime);

        FaceTarget();


        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime < canBlockTime && stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }
        Vector3 movement = CalculateMovement();

        if (normalizedTime < 1f)
        {
            
            //if(normalizedTime >= currentAttack.ForceTime)
            //{
            //    TryApplyForce();
            //}

        //    if (stateMachine.InputReader.IsAttacking)
        //    {
        //        if (hasCombo)
        //        {
        //            ComboAttack(normalizedTime);
        //        }
                

                

        //        if (stateMachine.Targeter.CurrentTarget != null && stateMachine.InputReader.IsTargeting) return;

        //        if (movement == Vector3.zero) { return; }

        //        FaceDirection(movement, Time.deltaTime);
        //    }

        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        
    }

    
    public override void Exit()
    {
        OnPlayerAttackComplete?.Invoke();
        stateMachine.Animator.applyRootMotion = false;
        stateMachine.InputReader.AttackPressed -= HandleAttackPressed;

    }
    private void ComboAttack(float normalizedTime)
    {
        if (normalizedTime < currentAttack.ComboAttackTime) { return; }
        OnPlayerAttackComplete?.Invoke();
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine,currentAttack.NextComboAttack));
    }

    void HandleAttackPressed()
    {
        // stateMachine.InputReader.AttackDown -= HandleAttackDown; //Makes it harder to time combos.
        // with this enabled, you CANNOT button mash and expect to combo.

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");
        ComboAttack(normalizedTime);
    }//Enables Button Push

    //private void TryApplyForce()
    //{
    //    if (alreadyAppliedForce) { return; }

    //    stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * currentAttack.Force);

    //    alreadyAppliedForce = true;
    //}



    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * stateMachine.RotationDamping);
    }

    
}
