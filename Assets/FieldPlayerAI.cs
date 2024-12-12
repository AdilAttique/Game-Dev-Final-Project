using UnityEngine;

namespace Assets.SuperGoalie.Scripts.Managers
{
public class FieldPlayerAI : MonoBehaviour
{
    public enum PlayerState { Attacking, Defending }
    public PlayerState CurrentState = PlayerState.Defending;

    [Header("Settings")]
    public float ReactionDistance = 10f; // Reaction distance to ball
    public Transform TeamGoal;
    public Transform OpponentGoal;
    public Transform Ball;
    public Transform PlayerWithBall;

    public Transform OptimalPosition;

    [Header("Support Settings")]
    public float SupportDistance = 6f; // Distance from the ball carrier
    public float SideOffset = 3f; // Side offset for diagonal movement
    public float OptimalPositionThreshold = 0.5f;
    public Transform StartingPosition;

    private UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter _thirdPersonCharacter;

    public GameObject gameManager;
    private GameManager gameManagerScript;

    private static FieldPlayerAI supportingPlayer; // Singleton for support role

    void Start()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        _thirdPersonCharacter = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();

        // Save starting position
        StartingPosition = new GameObject("StartingPosition").transform;
        StartingPosition.position = transform.position;
    }

    void Update()
    {
        // Assign state based on ball possession
        
        if (gameManagerScript.player.transform.CompareTag("Teammate"))
        {
            CurrentState = PlayerState.Attacking;
        }
        else
        {
            CurrentState = PlayerState.Defending;
        }

        // Execute behavior based on state
        switch (CurrentState)
        {
            case PlayerState.Attacking:
                HandleAttackingBehavior();
                break;

            case PlayerState.Defending:
                HandleDefendingBehavior();
                break;
        }
    }

    private void HandleAttackingBehavior()
    {
        if (PlayerWithBall != null)
        {
            // Assign this player as support if no one else is assigned
            if (supportingPlayer == null || supportingPlayer == this)
            {
                supportingPlayer = this;

                // Compute optimal position for support
                Vector3 optimalPosition = CalculateOptimalSupportPosition();
                MoveTowardsWithDeadZone(optimalPosition, OptimalPositionThreshold);
            }
            else
            {
                // Return to starting position if not supporting
                MoveTowardsWithDeadZone(OptimalPosition.position, OptimalPositionThreshold);
            }
        }
        else
        {
            // Return to starting position if no ball carrier
            MoveTowardsWithDeadZone(OptimalPosition.position, OptimalPositionThreshold);
        }
    }

    private void HandleDefendingBehavior()
    {
        // Release support role
        if (supportingPlayer == this)
        {
            supportingPlayer = null;
        }

        // Move defensively or reset to starting position
        MoveTowardsWithDeadZone(StartingPosition.position, OptimalPositionThreshold);
    }

    private Vector3 CalculateOptimalSupportPosition()
    {
        // Determine forward and side directions relative to the ball carrier
        Vector3 forwardDirection = PlayerWithBall.forward.normalized;
        Vector3 sideDirection = Vector3.Cross(Vector3.up, forwardDirection).normalized;

        // Calculate optimal support position with diagonal offset
        Vector3 optimalPosition = PlayerWithBall.position +
                                  (forwardDirection * SupportDistance) +
                                  (sideDirection * SideOffset);

        return optimalPosition;
    }

    private void MoveTowardsWithDeadZone(Vector3 targetPosition, float threshold)
    {
        // Smoothly approach the target position, avoiding jittering near the target
        if (Vector3.Distance(transform.position, targetPosition) > threshold)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            _thirdPersonCharacter.Move(direction, false, false);
        }
        else
        {
            // Stop movement when within the threshold
            _thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }
     public void ResetToStartingPosition()
        {
            transform.position = StartingPosition.position;
        }
    private void OnDisable()
    {
        // Release support role on disable
        if (supportingPlayer == this)
        {
            supportingPlayer = null;
        }
    }
}

}
