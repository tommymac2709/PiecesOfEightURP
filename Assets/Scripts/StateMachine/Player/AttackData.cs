using UnityEngine;
using System;

[Serializable]
public class AttackData
{
    [field: SerializeField] public string AnimationName {  get; private set; }

    [field: SerializeField] public float TransitionDuration { get; private set; }

    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;

    [field: SerializeField] public int DamageAmount { get; private set; }


    //How far through the animation a force is added
    [field: SerializeField] public float ForceTime { get; private set; }

    //Intensity of force (how much force is added)
    [field: SerializeField] public float Force { get; private set; }

    //How far through an attack it will let you combo into the next attack
    //I.e. if the player is still holding the attack button when the animtion is .8 of the way through, play the next attack
    [field: SerializeField] public float ComboAttackTime { get; private set; }

}
