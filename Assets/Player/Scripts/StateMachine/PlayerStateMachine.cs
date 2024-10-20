using GameDevTV.Saving;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerStateMachine : StateMachine, IJsonSaveable
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

    [field: SerializeField] public GameObject DeathUI { get; private set; }

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

    public event Action onSaveableEvent;

    private void Start()
    {
        //Inventory.Container.Clear();

        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));

        Health.onResurrection.AddListener(() =>
        {
            StartCoroutine(RespawnRoutine());
            
        });

    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(2f);
        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(2f);
        DeathUI.SetActive(true);
        yield return fader.FadeIn(2f);
        

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

    //public object CaptureState()
    //{
       
    //    return new SerializableVector3(Controller.transform.position);
    //}

    //public void RestoreState(object state)
    //{

    //    SerializableVector3 position = (SerializableVector3)state;
    //    Controller.enabled = false;
    //    Controller.transform.position = position.ToVector();
    //    Controller.enabled = true;
    //}

    public JToken CaptureAsJToken()
    {
        return Controller.transform.position.ToToken();
    }

    public void RestoreFromJToken(JToken state)
    {
        Controller.enabled = false;
        Controller.transform.position = state.ToVector3();
        Controller.enabled = true;
    }
}
