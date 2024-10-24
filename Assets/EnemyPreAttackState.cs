using UnityEngine;

public class EnemyPreAttackState : EnemyBaseState
{
    private readonly int CircleBlendTreeHash = Animator.StringToHash("CircleBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForwardSpeed");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRightSpeed");

    private const float CrossFadeDuration = 0.1f;
    private const float PreAttackDuration = 1f;
    private const float CircleChangeCooldownDuration = 2f; // Cooldown duration for circling direction change

    private float timeSpentPreAttacking;
    private float circleChangeCooldown;
    private bool hasDecidedToAttack;

    // Speed variables for different behaviors
    [SerializeField] private float moveTowardSpeed = 1.5f;
    [SerializeField] private float moveAwaySpeed = 1.5f;
    [SerializeField] private float diagonalBackwardSpeed = 1.5f;
    [SerializeField] private float circleSpeed = 0.5f;

    private enum PreAttackBehavior { CircleLeft, CircleRight, MoveToward, MoveAway, DiagonalBackward}
    private PreAttackBehavior chosenBehavior;
    private PreAttackBehavior previousBehavior;

    // Variables to hold current and target values for smooth transitions
    private float currentForwardValue;
    private float currentRightValue;
    private float targetForwardValue;
    private float targetRightValue;

    public EnemyPreAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {


        stateMachine.Animator.CrossFadeInFixedTime(CircleBlendTreeHash, CrossFadeDuration);
        timeSpentPreAttacking = 0;
        circleChangeCooldown = 0; // Reset cooldown
        hasDecidedToAttack = false;
        previousBehavior = chosenBehavior;
        ChooseRandomBehavior();

        // Initialize current and target values
        currentForwardValue = stateMachine.Animator.GetFloat(TargetingForwardHash);
        currentRightValue = stateMachine.Animator.GetFloat(TargetingRightHash);
        targetForwardValue = currentForwardValue;
        targetRightValue = currentRightValue;
    }

    public override void Tick(float deltaTime)
    {
        float distanceToPlayer = Vector3.Distance(stateMachine.transform.position, stateMachine.Player.transform.position);
        if (!IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        timeSpentPreAttacking += deltaTime;
        circleChangeCooldown -= deltaTime;

        AdjustBehaviorBasedOnDistance(deltaTime);

        // Smoothly interpolate between current and target values
        currentForwardValue = Mathf.Lerp(currentForwardValue, targetForwardValue, 0.1f); // Lerp to target forward value
        currentRightValue = Mathf.Lerp(currentRightValue, targetRightValue, 0.1f); // Lerp to target right value

        // Apply the lerped values to the animator
        stateMachine.Animator.SetFloat(TargetingForwardHash, currentForwardValue);
        stateMachine.Animator.SetFloat(TargetingRightHash, currentRightValue);

        Debug.Log(chosenBehavior);

        if (distanceToPlayer <= 2f && timeSpentPreAttacking >= PreAttackDuration && !hasDecidedToAttack)
        {
            hasDecidedToAttack = true;
            DecideToAttack();
        }
    }

    public override void Exit() { }

    private void ChooseRandomBehavior()
    {
        int randomChoice = Random.Range(0, 90); // 0-90 gives us 100 total options

        if (randomChoice < 50) // 50% chance to move toward player
        {
            chosenBehavior = PreAttackBehavior.MoveToward;
        }
        else if (randomChoice < 60) // 20% chance to circle left
        {
            chosenBehavior = PreAttackBehavior.CircleLeft;
        }
        else if (randomChoice < 65) // 20% chance to circle right
        {
            chosenBehavior = PreAttackBehavior.CircleRight;
        }
        else if (randomChoice < 85) // 10% chance to move away
        {
            chosenBehavior = PreAttackBehavior.MoveAway;
        }
        else // 10% chance to move diagonally backward
        {
            chosenBehavior = PreAttackBehavior.DiagonalBackward;
        }
    }

    private void AdjustBehaviorBasedOnDistance(float deltaTime)
    {
        float distanceToPlayer = Vector3.Distance(stateMachine.transform.position, stateMachine.Player.transform.position);


        // Handle movement based on proximity to player
        switch (chosenBehavior)
        {
            case PreAttackBehavior.MoveToward:
                if (distanceToPlayer <= 1.5f) // Stop if within 1 unit
                {
                    ChooseRandomCircle(); // Choose circling direction
                }
                else
                {
                    MoveTowardPlayer(deltaTime);
                }
                break;

            case PreAttackBehavior.MoveAway:
                if (distanceToPlayer >= 4.5f) // Stop if farther than n units
                {
                    ChooseRandomCircle(); // Choose circling direction
                }
                else
                {
                    MoveAwayFromPlayer(deltaTime);
                }
                break;

                      

            case PreAttackBehavior.DiagonalBackward:
                if (distanceToPlayer >= 4.5f) // Stop if too far
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
                // If the timer hits zero, choose a new random behavior
                if (circleChangeCooldown <= 0)
                {
                    ChooseRandomBehavior(); // Choose a new behavior
                }
                break;

            case PreAttackBehavior.CircleRight:
                CirclePlayer(deltaTime, false); // Circle right
                // If the timer hits zero, choose a new random behavior
                if (circleChangeCooldown <= 0)
                {
                    ChooseRandomBehavior(); // Choose a new behavior
                }
                break;
        }

        // Update previous behavior after switching
        previousBehavior = chosenBehavior;
    }

    private void ChooseRandomCircle()
    {
        if (circleChangeCooldown <= 0) // Only change direction if cooldown is complete
        {
            chosenBehavior = (Random.Range(0, 2) == 0) ? PreAttackBehavior.CircleLeft : PreAttackBehavior.CircleRight;
            circleChangeCooldown = Random.Range(2f,5f); // Reset the cooldown timer
        }
    }

    private void CirclePlayer(float deltaTime, bool circleLeft)
    {
        Vector3 direction = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
        Vector3 circleDirection = circleLeft ? Vector3.Cross(direction, Vector3.up) : Vector3.Cross(Vector3.up, direction); // Left or right circle

        Move(circleDirection * circleSpeed, deltaTime); // Use circling speed
        FaceTarget(stateMachine.Player.transform.position, deltaTime); // Always face the player

        // Set target animator parameters for circling
        targetForwardValue = 0.5f; // Adjust for speed
        targetRightValue = circleLeft ? -0.5f : 0.5f; // Circle left or right
    }

    private void MoveTowardPlayer(float deltaTime)
    {
        Vector3 direction = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
        Move(direction * moveTowardSpeed, deltaTime); // Move closer with specified speed
        FaceTarget(stateMachine.Player.transform.position, deltaTime);

        // Set target animator parameters for moving toward the player
        targetForwardValue = 1f; // Forward movement
        targetRightValue = 0f; // No right movement
    }

    private void MoveAwayFromPlayer(float deltaTime)
    {
        Vector3 direction = (stateMachine.transform.position - stateMachine.Player.transform.position).normalized;
        Move(direction * moveAwaySpeed, deltaTime); // Move away with specified speed

        FaceTarget(stateMachine.Player.transform.position, deltaTime);

        // Set target animator parameters for moving away from the player
        targetForwardValue = -1f; // Backward movement
        targetRightValue = 0f; // No right movement
    }

    private void MoveDiagonalBackward(float deltaTime)
    {
        Vector3 directionToPlayer = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;

        // Calculate a diagonal backward direction (45 degrees)
        Vector3 diagonalBackwardDirection = new Vector3(-directionToPlayer.x, 0, -directionToPlayer.z).normalized * 0.7071f; // 0.7071 is approx 1/sqrt(2) for 45-degree angle

        Move(diagonalBackwardDirection * diagonalBackwardSpeed, deltaTime); // Move diagonally backward
        FaceTarget(stateMachine.Player.transform.position, deltaTime);

        // Set target animator parameters for moving diagonally backward
        targetForwardValue = -0.5f; // Adjust backward speed
        targetRightValue = -0.5f; // Adjust for diagonal movement
    }

    private void DecideToAttack()
    {
        bool shouldAttack = Random.Range(0, 5) < 4; // 80% chance to attack

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
