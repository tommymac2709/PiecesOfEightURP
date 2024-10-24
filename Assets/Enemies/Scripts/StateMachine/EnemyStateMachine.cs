using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine, ISaveable
{
    [field: SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public BaseStats BaseStats { get; private set; }

    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public Fighter Fighter { get; private set; }

    [field: SerializeField] public Health Health{ get; private set; }

    [field: SerializeField] public Health ThisHealth { get; private set; }

    [SerializeField] public PlayerStateMachine PlayerStateMachine { get; private set; }

    [field: SerializeField] public NavMeshAgent Agent { get; private set; }

   // [field: SerializeField] public Ragdoll Ragdoll { get; private set; }

    [field: SerializeField] public Target Target { get; private set; }

    [field: SerializeField] public CooldownTokenManager CooldownTokenManager { get; private set; }

    [field: SerializeField] public PatrolPath PatrolPath { get; private set; }


    //[field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; }

    [field: SerializeField] public float RotationSpeed { get; private set; }

    [field: SerializeField] public float ImpactStateDuration { get; private set; }

    [field: SerializeField] public float ParriedStateDuration { get; private set; }

    [field: SerializeField] public float WanderRadius { get; private set; }

    [field: SerializeField] public float WanderTimer { get; private set; }

    [field: SerializeField] public float AttackRange { get; private set; }

    [field: SerializeField] public int AttackDamage { get; private set; }

    [field: SerializeField] public float AttackKnockback { get; private set; }

    [field: SerializeField] public float PlayerDetectRange { get; private set; }

    [field: SerializeField] public float PlayerFleeRange { get; private set; }

    public Health Player { get; private set; }
    public AbilityManager PlayerAbilityManager { get; private set; }

    public bool IsAttacking { get; private set; }

    public Blackboard Blackboard = new Blackboard();
    private void Start()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        PlayerStateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        PlayerAbilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        
        SwitchState(new EnemyIdleState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
        Health.OnLowHealth += FleeFromPlayer;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
        Health.OnLowHealth -= FleeFromPlayer;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this, ImpactStateDuration));
    }

    private void FleeFromPlayer()
    {
        bool shouldFlee = Random.Range(0, 2) == 0; // 50% chance to attack
        if (shouldFlee)
        {
            SwitchState(new EnemyFleeState(this));
        }
       
        
    }

    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }

    public void SetAttacking(bool isAttacking)
    {
        IsAttacking = isAttacking;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, PlayerFleeRange);
    }

    [System.Serializable]
    struct StateMachineData
    {
        public SerializableVector3 position;
        public SerializableVector3 rotation;
    }

    public object CaptureState()
    {
        StateMachineData data = new StateMachineData();
        data.position = new SerializableVector3(transform.position);
        data.rotation = new SerializableVector3(transform.eulerAngles);
        return data;
    }

    public void RestoreState(object state)
    {

        StateMachineData data = (StateMachineData)state;
        Agent.enabled = false;
        transform.position = data.position.ToVector();
        transform.eulerAngles = data.rotation.ToVector();
        Agent.enabled = true;
    }


}
