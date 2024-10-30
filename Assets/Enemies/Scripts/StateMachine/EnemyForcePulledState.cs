using Unity.VisualScripting;
using UnityEngine;

public class EnemyForcePulledState : EnemyBaseState
{
    private readonly int ForcePullAnimHash = Animator.StringToHash("ForcePull");
    private const float CrossFadeDuration = 0.1f;

    private float speed = 10f;
    public EnemyForcePulledState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Controller.enabled = false;
        stateMachine.Animator.CrossFadeInFixedTime(ForcePullAnimHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
        
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.transform.position = Vector3.MoveTowards(stateMachine.transform.position, stateMachine.Player.transform.position, deltaTime * speed);
        if (Vector3.Distance(stateMachine.transform.position, stateMachine.Player.transform.position) < 2f)
        {
            stateMachine.Controller.enabled = true;
            //stateMachine.SwitchState(new EnemyImpactState(stateMachine,));
            return;
        }

    }

}
