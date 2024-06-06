using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public NavMeshAgent Agent { get; private set; }

    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; }

    [field: SerializeField] public float WanderRadius { get; private set; }

    [field: SerializeField] public float WanderTimer { get; private set; }

    [field: SerializeField] public float AttackRange { get; private set; }

    [field: SerializeField] public int AttackDamage { get; private set; }

    [field: SerializeField] public float PlayerDetectRange { get; private set; }

    [field: SerializeField] public float PlayerFleeRange { get; private set; }

    public GameObject Player { get; private set; }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Agent.updatePosition = false;
        Agent.updateRotation = false;
        
        SwitchState(new EnemyIdleState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, PlayerFleeRange);
    }


}
