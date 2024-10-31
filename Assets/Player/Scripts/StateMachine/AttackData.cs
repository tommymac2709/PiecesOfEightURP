using UnityEngine;
using System;


[CreateAssetMenu(fileName = "AttackData", menuName = "Combat/New Attack", order = 0)]
public class AttackData : ScriptableObject

{
    [field: SerializeField] public bool ApplyRootMotion { get; private set; } = true;

    [field: SerializeField] public bool ApplyImpactRootMotion { get; private set; } = true;

    [field: SerializeField] public string AnimationName {  get; private set; }

    [field: SerializeField] public string ImpactAnimationName { get; private set; }


    [Range(0.5f, 2.0f)]
    [Tooltip("This will be multiplied by the damage in the Current Weapon Config")]
    [SerializeField]
    private float damageModifier = 1.0f;


    [Range(0, 1)] [SerializeField] private float transitionDuration = .1f;

    [field: SerializeField] public AttackData NextComboAttack { get; private set; }


    [field: SerializeField] public float Cooldown { get; private set; } = 1f;


    //How far through an attack it will let you combo into the next attack
    //I.e. if the player is still holding the attack button when the animtion is .8 of the way through, play the next attack

    [Range(0, 0.99f)][SerializeField] private float comboAttackTime = 0.99f;



    public float DamageModifier => damageModifier;
    public float TransitionDuration => transitionDuration;
    public float ComboAttackTime => comboAttackTime;


    //[field: SerializeField] public int DamageAmount { get; private set; }

    ////How far through the animation a force is added
    //[field: SerializeField] public float ForceTime { get; private set; }

    ////Intensity of force (how much force is added)
    //[field: SerializeField] public float Force { get; private set; }



    [field: SerializeField] public float Knockback { get; private set; }

}
