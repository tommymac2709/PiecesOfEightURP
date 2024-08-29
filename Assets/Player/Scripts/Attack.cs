using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Combat/New Attack", order = 0)]
public class Attack : ScriptableObject
{
    [field: SerializeField] public string AnimationName { get; private set; } = "Attack";
    [field: SerializeField] public bool ApplyRootMotion { get; private set; } = true;

    [Range(0.5f, 2.0f)]
    [Tooltip("This will be multiplied by the damage in the Current Weapon Config")]
    [SerializeField]
    private float damageModifier = 1.0f;

    [Range(0, 1)][SerializeField] private float transitionDuration = .1f;

    [field: SerializeField] public Attack NextComboAttack { get; private set; }
    [Range(0, 0.99f)][SerializeField] private float comboAttackTime = 0.99f;

    public float DamageModifier => damageModifier;
    public float TransitionDuration => transitionDuration;
    public float ComboAttackTime => comboAttackTime;
}

