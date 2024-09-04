using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public Fighter Fighter { get; private set; }

    [field: SerializeField] public Health Health{ get; private set; }

    [field: SerializeField] public NavMeshAgent Agent { get; private set; }

    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }

    [field: SerializeField] public Target Target { get; private set; }

    //[field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; }

    [field: SerializeField] public float RotationSpeed { get; private set; }

    [field: SerializeField] public float ImpactStateDuration { get; private set; }

    [field: SerializeField] public float WanderRadius { get; private set; }

    [field: SerializeField] public float WanderTimer { get; private set; }

    [field: SerializeField] public float AttackRange { get; private set; }

    [field: SerializeField] public int AttackDamage { get; private set; }

    [field: SerializeField] public float AttackKnockback { get; private set; }

    [field: SerializeField] public float PlayerDetectRange { get; private set; }

    [field: SerializeField] public float PlayerFleeRange { get; private set; }

    public Health Player { get; private set; }
    public AbilityManager PlayerAbilityManager { get; private set; }

    private void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        PlayerAbilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        
        SwitchState(new EnemyIdleState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this, ImpactStateDuration));
    }

    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, PlayerFleeRange);
    }


}
