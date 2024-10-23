using UnityEngine;

public class EnemyPreAttackState : EnemyBaseState
{
    private readonly int CircleBlendTreeHash = Animator.StringToHash("CircleBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForwardSpeed");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRightSpeed");

    private const float CrossFadeDuration = 0.1f;
    private const float PreAttackDuration = 1.5f;
    private const float CircleChangeCooldownDuration = 2f; // Cooldown duration for circling direction change

    private float timeSpentPreAttacking;
    private float circleChangeCooldown;
    private bool hasDecidedToAttack;

    // Speed variables for different behaviors
    [SerializeField] private float moveTowardSpeed = 0.5f;
    [SerializeField] private float moveAwaySpeed = 0.5f;
    [SerializeField] private float diagonalBackwardSpeed = 0.5f; // New speed for diagonal backward
    [SerializeField] private float circleSpeed = 0.5f;

    private enum PreAttackBehavior { CircleLeft, CircleRight, MoveToward, MoveAway, DiagonalBackward }
    private PreAttackBehavior chosenBehavior;

    // Current animator parameter values
    private float currentForwardValue;
    private float currentRightValue;

    public EnemyPreAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(CircleBlendTreeHash, 0.5f);
        timeSpentPreAttacking = 0;
        circleChangeCooldown = 0; // Reset cooldown
        hasDecidedToAttack = false;
        ChooseRandomBehavior();

        // Initialize current values
        currentForwardValue = 0f;
        currentRightValue = 0f;
    }

    public override void Tick(float deltaTime)
    {
        if (!IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        timeSpentPreAttacking += deltaTime;
        circleChangeCooldown -= deltaTime; // Decrease cooldown timer

        AdjustBehaviorBasedOnDistance(deltaTime);

        // Smoothly update animator parameters
        stateMachine.Animator.SetFloat(TargetingForwardHash, Mathf.Lerp(stateMachine.Animator.GetFloat(TargetingForwardHash), currentForwardValue, 0.1f));
        stateMachine.Animator.SetFloat(TargetingRightHash, Mathf.Lerp(stateMachine.Animator.GetFloat(TargetingRightHash), currentRightValue, 0.1f));

        if (timeSpentPreAttacking >= PreAttackDuration && !hasDecidedToAttack)
        {
            hasDecidedToAttack = true;
            DecideToAttack();
        }
    }

    public override void Exit() { }

    private void ChooseRandomBehavior()
    {
        int randomChoice = Random.Range(0, 5); // Randomly choose from the new range
        chosenBehavior = (PreAttackBehavior)randomChoice;
    }

    private void AdjustBehaviorBasedOnDistance(float deltaTime)
    {
        float distanceToPlayer = Vector3.Distance(stateMachine.transform.position, stateMachine.Player.transform.position);

        // Handle movement based on proximity to player
        switch (chosenBehavior)
        {
            case PreAttackBehavior.MoveToward:
                if (distanceToPlayer <= 2.5f) // Stop if within 1 unit
                {
                    ChooseRandomCircle(); // Choose circling direction
                }
                else
                {
                    MoveTowardPlayer(deltaTime);
                }
                break;

            case PreAttackBehavior.MoveAway:
                if (distanceToPlayer >= 3f) // Stop if farther than 2 units
                {
                    ChooseRandomCircle(); // Choose circling direction
                }
                else
                {
                    MoveAwayFromPlayer(deltaTime);
                }
                break;

            case PreAttackBehavior.DiagonalBackward:
                if (distanceToPlayer >= 2f) // Stop if too far
                {
                    ChooseRandomCircle(); // Choose circling direction
                }
                else
                {
                    MoveDiagonalBackward(deltaTime);
                }
                break;

            case PreAttackBehavior.CircleLeft:
                CirclePlayer(deltaTime, true); // Circle left
                break;

            case PreAttackBehavior.CircleRight:
                CirclePlayer(deltaTime, false); // Circle right
                break;
        }
    }

    private void ChooseRandomCircle()
    {
        if (circleChangeCooldown <= 0) // Only change direction if cooldown is complete
        {
            chosenBehavior = (Random.Range(0, 2) == 0) ? PreAttackBehavior.CircleLeft : PreAttackBehavior.CircleRight;
            circleChangeCooldown = CircleChangeCooldownDuration; // Reset the cooldown timer
        }
    }

    private void CirclePlayer(float deltaTime, bool circleLeft)
    {
        Vector3 direction = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
        Vector3 circleDirection = circleLeft ? Vector3.Cross(direction, Vector3.up) : Vector3.Cross(Vector3.up, direction); // Left or right circle

        Move(circleDirection * circleSpeed, deltaTime); // Use circling speed
        FaceTarget(stateMachine.Player.transform.position, deltaTime); // Always face the player

        // Set current animator parameters for circling
        currentForwardValue = 0.5f; // Adjust for speed
        currentRightValue = circleLeft ? -0.5f : 0.5f; // Circle left or right
    }

    private void MoveTowardPlayer(float deltaTime)
    {
        Vector3 direction = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
        Move(direction * moveTowardSpeed, deltaTime); // Move closer with specified speed
        FaceTarget(stateMachine.Player.transform.position, deltaTime);

        // Set current animator parameters for moving toward the player
        currentForwardValue = 1f; // Forward movement
        currentRightValue = 0f; // No right movement
    }

    private void MoveAwayFromPlayer(float deltaTime)
    {
        Vector3 direction = (stateMachine.transform.position - stateMachine.Player.transform.position).normalized;
        Move(direction * moveAwaySpeed, deltaTime); // Move away with specified speed

        // Ensure we don't move too far
        if (Vector3.Distance(stateMachine.transform.position, stateMachine.Player.transform.position) >= 2f)
        {
            ChooseRandomCircle(); // Switch to circling when too far
        }

        FaceTarget(stateMachine.Player.transform.position, deltaTime);

        // Set current animator parameters for moving away from the player
        currentForwardValue = -1f; // Backward movement
        currentRightValue = 0f; // No right movement
    }

    private void MoveDiagonalBackward(float deltaTime)
    {
        Vector3 directionToPlayer = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;

        // Calculate a diagonal backward direction (45 degrees)
        Vector3 diagonalBackwardDirection = new Vector3(-directionToPlayer.x, 0, -directionToPlayer.z).normalized * 0.7071f; // 0.7071 is approx 1/sqrt(2) for 45-degree angle

        Move(diagonalBackwardDirection * diagonalBackwardSpeed, deltaTime); // Move diagonally backward
        FaceTarget(stateMachine.Player.transform.position, deltaTime);

        // Set current animator parameters for moving diagonally backward
        currentForwardValue = -0.5f; // Adjust backward speed
        currentRightValue = -0.5f; // Adjust for diagonal movement
    }

    private void DecideToAttack()
    {
        bool shouldAttack = Random.Range(0, 2) == 0; // 50% chance to attack

        if (shouldAttack && !IsOtherEnemyAttacking())
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine)); // Attack if allowed
        }
        else
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine)); // Otherwise, go back to chasing
        }
    }
}
