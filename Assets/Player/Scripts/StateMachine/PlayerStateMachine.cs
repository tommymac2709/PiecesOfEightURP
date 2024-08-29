using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public GameObject ClimbCamera { get; private set; }

    [field: SerializeField] public InputReader InputReader { get; private set; }

    [field: SerializeField] public Fighter Fighter { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }

    [field: SerializeField] public AbilityManager AbilityManager { get; private set; }
    //[field: SerializeField] public AttackData[] AttackData { get; private set; }

    [field: SerializeField] public DamageReceiver DamageReceiver { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public Health Health{ get; private set; }

    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }

    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }

    [field: SerializeField] public WeaponDamage WeaponDamage { get; private set; }

    [field: SerializeField] public GameObject TargetCamera { get; private set; }

    [field: SerializeField] public GameObject FreeLookCamera { get; private set; }

    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }

    [field: SerializeField] public float FreeLookSprintMovementSpeed { get; private set; }

    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }

    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeDurationFreeLook { get; private set; }

    [field: SerializeField] public float DodgeDistance { get; private set; }

    [field: SerializeField] public float DodgeDistanceFreeLook { get; private set; }

    [field: SerializeField] public float DodgeCooldown { get; private set; }

    [field: SerializeField] public float DodgeInvulnerabilityDurationFreeLook { get; private set; }

    [field: SerializeField] public float DodgeInvulnerabilityDurationTargeting { get; private set; }

    [field: SerializeField] public float JumpForce { get; private set; }

    [field: SerializeField] public float ImpactStateDuration { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }

    public bool IsInvisible { get; private set; }

    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;
    public Transform MainCameraTransform { get; private set; }

    private void Start()
    {
        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));
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
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }

    public void SetDodgeTime(float dodgeTime)
    {
        PreviousDodgeTime = dodgeTime;
    }
}
