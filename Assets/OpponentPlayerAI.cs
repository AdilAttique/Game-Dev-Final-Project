using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Assets.SuperGoalie.Scripts.Managers
{
    public class OpponentPlayerAI : MonoBehaviour
    {
        public enum TeamState { Defending, Attacking }
        public TeamState CurrentTeamState = TeamState.Defending;
        private bool hasScoredGoal = false;
        [Header("Settings")]
        public Transform Ball; // Ball position 
        
        public Transform TeamGoal;
        public Transform OptimalPosition; // Player's optimal support position
        public float StealDistance = 0; // Distance to reach the ball
        public float SupportDistance = 6f; // Distance to support teammates
        public float OptimalPositionThreshold = 0.5f; // Dead zone threshold for optimal position

        public delegate void BallLaunch(float power, Vector3 target);  // Delegate to launch a ball
        public BallLaunch OnBallLaunch;                              // Event for ball launch

        public float SideOffset = 3f;
            private Vector3 startingPosition; // Initial position of the player
            private static OpponentPlayerAI activeStealer; // The player currently attempting to reach the ball
            public GameObject gameManager;
            private GameManager gameManagerScript;
        private ThirdPersonCharacter thirdPersonCharacter; // Third-person movement controller

                public GameObject goalTrigger;
                private float stealCooldownTimer = 0f; // Cooldown timer for reassigning the stealer
private const float StealCooldownDuration = 1f; // Duration for cooldown
        void Start()
        {
            
            gameManagerScript = gameManager.GetComponent<GameManager>();
            OnBallLaunch += gameManagerScript._ball.Instance_OnBallLaunch;
            // Save the starting position of the player
            startingPosition = transform.position;
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        }
         public void ResetToStartingPosition()
        {
            transform.position = startingPosition;

        }
        void Update()
        {
              if (stealCooldownTimer > 0f)
                stealCooldownTimer -= Time.deltaTime;
            // Determine the team state (example logic, adjust as needed)
            if(gameManagerScript.player != null)
            if (gameManagerScript.player.transform.CompareTag("Opponent"))
            {
                CurrentTeamState = TeamState.Attacking;
            }
            else
            {
                CurrentTeamState = TeamState.Defending;
            }

            if (gameManagerScript.player == null) // No player has possession
         {
        //CurrentTeamState = TeamState.Defending;
        HandleBallFreeState();
        }
        else
            switch (CurrentTeamState)
            {
                case TeamState.Defending:
                    HandleDefensiveBehavior();
                    break;

                case TeamState.Attacking:
                    HandleOffensiveBehavior();
                    break;
            }
        }
    
    private void HandleBallFreeState()
{
    // Assign the nearest player to actively approach the ball if no one is assigned or cooldown is over
    if (activeStealer == null || (activeStealer == this && stealCooldownTimer <= 0f))
    {
        activeStealer = FindClosestOpponentToBall();
        stealCooldownTimer = StealCooldownDuration; // Reset cooldown timer
    }

    if (activeStealer == this)
    {
        Debug.Log("Approaching the free ball!");
        if (Vector3.Distance(transform.position, Ball.position) > 0.01f)
        {
            Vector3 direction = (Ball.position - transform.position).normalized;
            direction.x += Random.Range(-0.5f, 0.5f); // Randomize X component for dynamic movement
            direction.z += Random.Range(-0.5f, 0.5f); // Randomize Z component for dynamic movement
            direction = direction.normalized; // Normalize to keep speed constant

            MoveTowards(Ball.position + direction); // Move with random offset
        }
        else
        {
            StopMovement(); // Stop if close enough
            activeStealer = null; // Release the active stealer role
        }
    }
    else
    {
        // Non-stealers return to their starting positions
        if (Vector3.Distance(transform.position, startingPosition) > 2f)
            MoveTowards(startingPosition);
        else
            WanderAroundStartingPosition();
    }
}
private void HandleDefensiveBehavior()
{
    // Nearest player to the ball moves towards it
    if (activeStealer == null || activeStealer == this)
    {
        activeStealer = FindClosestOpponentToBall();

        if (Vector3.Distance(transform.position, Ball.position) > 0.01f)
        {
            // Randomize approach angle for more dynamic behavior
            Vector3 direction = (Ball.position - transform.position).normalized;
            direction.x += Random.Range(-0.5f, 0.5f); // Randomize X component for dynamic movement
            direction.z += Random.Range(-0.5f, 0.5f); // Randomize Z component for dynamic movement
            direction = direction.normalized; // Normalize to keep speed constant

            MoveTowards(Ball.position + direction); // Move with random offset
        }
        else
        {
            StopMovement();
        }
    }
    else
    {
        // Other players return to their starting positions
       if (Vector3.Distance(transform.position,startingPosition) > 2f)
            MoveTowards(startingPosition);
        else
        WanderAroundStartingPosition();
    }
}
    private OpponentPlayerAI FindClosestOpponentToBall()
        {
            OpponentPlayerAI closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (OpponentPlayerAI opponent in FindObjectsOfType<OpponentPlayerAI>())
            {
                float distance = Vector3.Distance(opponent.transform.position, Ball.position);
                if (distance < closestDistance && opponent != activeStealer)
                {
                    closestDistance = distance;
                    closestPlayer = opponent;
                }
            }

            return closestPlayer;
        }
        private void HandleOffensiveBehavior()
        {
            Transform teammateWithBall = null;
            if(gameManagerScript.player != null)
             teammateWithBall = gameManagerScript.player.transform;
            if(teammateWithBall == this.gameObject.transform)
            {
                if (Vector3.Distance(transform.position, TeamGoal.position) > 20)
                {
                    MoveTowards(TeamGoal.position);
                    
                     Vector3 forwardOffset = gameManagerScript.player.transform.forward * 0.8f; // 0.8 units in the forward direction
                    Vector3 targetPosition = gameManagerScript.player.transform.position + forwardOffset;

                    Ball.transform.position = targetPosition;
                }
                else
                {
                    
                        
                    gameManagerScript.animator.Play("Pass");

    // Define the goal area boundaries
                    Bounds goalBounds = goalTrigger.GetComponent<Collider>().bounds;

                    // Randomly select a point within the goal area
                    Vector3 randomGoalPoint = new Vector3(
                        Random.Range(-3.5f, 3.5f),
                        Random.Range(0, 2),
                        goalBounds.center.z // Assume shooting is along the Z-axis
                    );
    
    // Launch the ball towards the randomly selected goal point
                    BallLaunch tempBallLaunch = OnBallLaunch;
                    if (tempBallLaunch != null)
                    {
        // Adjust the launch force if needed
                        tempBallLaunch?.Invoke(15, randomGoalPoint);
                    }

    // Optionally, reset the ball or enable goal triggers

                    gameManagerScript.player = null;
             //   gameManagerScript.animator.Play("Celebrate");
               
                    
    // StartCoroutine(Reset()); // Uncomment if you have a reset mechanism
                }
            }
            else if (teammateWithBall != null)
            {
                if (Vector3.Distance(transform.position, teammateWithBall.position) < SupportDistance)
                {
                    // Support the teammate by maintaining proximity
                    Vector3 optimalPosition = CalculateOptimalSupportPosition();
                    MoveTowards(optimalPosition);
                }
                else
                {
                    // Move to optimal position
                    MoveTowards(OptimalPosition.position);
                }
            }
            else
            {
                // Return to optimal position if no ball carrier
                MoveTowards(OptimalPosition.position);
            }
        }

private void WanderAroundStartingPosition()
        {
            // Randomly wander near the starting position
            Vector3 randomOffset = new Vector3(
                Random.Range(-2f, 2f),
                0,
                Random.Range(-2f, 2f)
            );
            Vector3 wanderTarget = startingPosition + randomOffset;

            MoveTowards(wanderTarget);
        }
        private Vector3 CalculateOptimalSupportPosition()
    {
        // Determine forward and side directions relative to the ball carrier
        Vector3 forwardDirection = gameManagerScript.player.transform.forward.normalized;
        Vector3 sideDirection = Vector3.Cross(Vector3.up, forwardDirection).normalized;

        // Calculate optimal support position with diagonal offset
        Vector3 optimalPosition = gameManagerScript.player.transform.position +
                                  (forwardDirection * SupportDistance) +
                                  (sideDirection * SideOffset);

        return optimalPosition;
    }
        private void MoveTowards(Vector3 targetPosition)
        {
            if (Vector3.Distance(transform.position, targetPosition) > OptimalPositionThreshold)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                thirdPersonCharacter.Move(direction, false, false);
            }
            else
            {
                StopMovement();
            }
        }

        private void StopMovement()
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if this player collided with the ball
            if (other.transform == Ball)
            {
                Debug.Log($"{name} has collided with the ball!");
                activeStealer = null; // Reset stealer role after collision
            }
        }

        private void OnDisable()
        {
            if (activeStealer == this)
            {
                activeStealer = null; // Reset stealer role on disable
            }
        }
    }
}
