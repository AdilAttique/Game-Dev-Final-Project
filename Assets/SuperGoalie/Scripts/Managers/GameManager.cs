using Assets.SuperGoalie.Scripts.Entities;
using Assets.SuperGoalie.Scripts.States.GoalKeeperStates.Idle.MainState;
using Assets.SuperGoalie.Scripts.States.GoalKeeperStates.Idle.SubStates;
using Patterns.Singleton;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Assets.SuperGoalie.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        float _ballDribbleForce = 5f;

        [SerializeField]
        float _ballKickForce = 15;

        public Ball _ball;

        [SerializeField]
        Goal _goal;

         [SerializeField]
        Goal _goal2;

        [SerializeField]
        GoalKeeper _goalKeeper1;  // First Goalkeeper
        [SerializeField]
        GoalKeeper _goalKeeper2;  // Second Goalkeeper

        [SerializeField]
        Text _scoreText;

        int _score;
        Vector3 _ballInitPos;
        Quaternion _ballInitRot;

        protected Transform Cam;                  // A reference to the main camera in the scenes transform
        protected Vector3 CamForward;             // The current forward direction of the camera

        public delegate void BallLaunch(float power, Vector3 target);   //delegate to launch a ball
        public BallLaunch OnBallLaunch;                                 //on ball launch

        public bool hasPossesion = false;

        public GameObject player;

        public Animator animator;

        public UnityEngine.UI.Text Team1;
        public UnityEngine.UI.Text Team2;

        public override void Awake()
        {
            if(PlayerPrefs.HasKey("teamAName"))
            Team1.text = PlayerPrefs.GetString("teamAName");
            if(PlayerPrefs.HasKey("teamBName"))
            Team2.text = PlayerPrefs.GetString("teamBName");

            // register the game manager to some events
            animator = player.GetComponent<Animator>();
            _ball.OnBallLaunched += SoundManager.Instance.PlayBallKickedSound;
            _goalKeeper1.OnPunchBall += SoundManager.Instance.PlayBallKickedSound;
            _goalKeeper2.OnPunchBall += SoundManager.Instance.PlayBallKickedSound;
            //_goal.GoalTrigger.OnCollidedWithBall += SoundManager.Instance.PlayGoalScoredSound;

            //register entities to entity delegates
            _ball.OnBallLaunched += _goalKeeper1.Instance_OnBallLaunched;
            _ball.OnBallLaunched += _goalKeeper2.Instance_OnBallLaunched;

            //register entities to local delegates
            OnBallLaunch += _ball.Instance_OnBallLaunch;

            //cache the initial data
            _ballInitPos = _ball.Position;
            _ballInitRot = _ball.Rotation;
            // get the transform of the main camera
            if (Camera.main != null)
                Cam = Camera.main.transform;
            else
                Debug.LogWarning("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        private void Instance_OnBallCollidedWithGoal()
        {
            ++_score;
            _scoreText.text = string.Format("Score:{0}", _score);
        }

        private GameObject FindGameObjectInScene(string name)
        {
            GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allGameObjects)
            {
                if (obj.name == name && obj.scene.isLoaded) // Ensure the object is in the current scene
                {
                    return obj;
                }
            }
            return null; // Return null if not found
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Check if the team is defending
                if (!hasPossesion)
                {
                    // Identify the current player by checking which player has the user control script enabled
                    GameObject currentPlayer = null;

                    // Find all players with the "ThirdPersonUserControl" component
                    ThirdPersonUserControl[] allPlayers = FindObjectsOfType<ThirdPersonUserControl>();
                    foreach (ThirdPersonUserControl control in allPlayers)
                    {
                        if (control.enabled)
                        {
                            currentPlayer = control.gameObject;
                            break;
                        }
                    }

                    if (currentPlayer == null)
                    {
                        Debug.LogError("No player with 'ThirdPersonUserControl' enabled found!");
                        return;
                    }

                    // Get the current player's transform and facing direction
                    Transform currentPlayerTransform = currentPlayer.transform;
                    Vector3 forwardDirection = currentPlayerTransform.forward;

                    float maxAngle = 45f; // Allowable angle range to consider players in front
                    float closestDistance = float.MaxValue;
                    GameObject closestPlayer = null;

                    // Find all players tagged as teammates
                    GameObject[] teammates = GameObject.FindGameObjectsWithTag("Teammate");
                    foreach (GameObject teammate in teammates)
                    {
                        if (teammate == currentPlayer)
                            continue; // Skip the current player

                        Vector3 directionToTeammate = (teammate.transform.position - currentPlayerTransform.position).normalized;
                        float angleToTeammate = Vector3.Angle(forwardDirection, directionToTeammate);

                        // Check if the teammate is within the allowable angle
                        if (angleToTeammate <= maxAngle)
                        {
                            float distance = Vector3.Distance(currentPlayerTransform.position, teammate.transform.position);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestPlayer = teammate;
                            }
                        }
                    }

                    // Switch control to the closest teammate
                    if (closestPlayer != null)
                    {
                        // Disable AI on the current player
                        currentPlayer.GetComponent<ThirdPersonUserControl>().enabled = false;
                        currentPlayer.GetComponent<FieldPlayerAI>().enabled = true;

                        // Update the GameManager's player
                        player = closestPlayer;
                        animator = player.GetComponent<Animator>();

                        // Enable user control on the new player
                        player.GetComponent<ThirdPersonUserControl>().enabled = true;
                        player.GetComponent<FieldPlayerAI>().enabled = false;

                        // Reparent the camera follower to the new player
                        GameObject cameraFollower = FindGameObjectInScene("CameraFollower");
                        if (cameraFollower != null)
                        {
                            cameraFollower.transform.SetParent(player.transform);
                            cameraFollower.transform.localPosition = new Vector3(0f, 2.3f, -4.46f);
                            cameraFollower.transform.localRotation = Quaternion.Euler(10f, 0f, 0f);
                        }

                        Debug.Log($"Switched control to {closestPlayer.name}");
                    }
                    else
                    {
                        Debug.Log("No suitable teammate found in the facing direction.");
                    }
                }
            }

            if (hasPossesion)
            {
                Vector3 forwardOffset = player.transform.forward * 0.8f; // 0.8 units in the forward direction
                Vector3 targetPosition = player.transform.position + forwardOffset;

                // Update the ball's position
                _ball.transform.position = targetPosition;
            }

            #region TriggerShooting

            //get the mouse
            if (Input.GetMouseButtonDown(0) && hasPossesion)
            {
                // Create a ray from the mouse clicked position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Run a raycast into the scene
                if (Physics.Raycast(ray, out hit))
                {
                    hasPossesion = false;
                    animator.Play("Pass");

                    // Get the target point where the mouse click intersected the scene
                    Vector3 target = hit.point;

                    // Launch the ball towards the target point
                    BallLaunch tempBallLaunch = OnBallLaunch;
                    if (tempBallLaunch != null)
                    {
                        // You can apply logic here to adjust the launch force
                        tempBallLaunch.Invoke(_ballKickForce, target);
                    }

                    // Optionally, you can reset the ball after a delay
                //_goalKeeper.FSM.ChangeState<IdleMainState>();
                    //animator.Play("Celebrate");
                    
                    
                
                }
            }
        }

            #endregion
        // private IEnumerator Reset()
        // {
           
        //     Debug.Log("resetting");
        //     yield return new WaitForSeconds(5f);
        //     SceneManager.LoadScene("Main Scene");
        //     // GameObject[] players = GameObject.FindGameObjectsWithTag("Teammate");
        //     // GameObject[] opponents = GameObject.FindGameObjectsWithTag("Opponent");

        //     // foreach (GameObject player in players)
        //     // {
        //     //     FieldPlayerAI playerAI = player.GetComponent<FieldPlayerAI>();
        //     //     if (playerAI != null)
        //     //     {
        //     //         playerAI.ResetToStartingPosition();
        //     //     }
        //     // }

        //     // foreach (GameObject opponent in opponents)
        //     // {
        //     //     OpponentPlayerAI opponentAI = opponent.GetComponent<OpponentPlayerAI>();
        //     //     if (opponentAI != null)
        //     //     {
        //     //         opponentAI.ResetToStartingPosition();
        //     //     }
        //     // }
        //     // _ball.gameObject.SetActive(false);
        //     // _ball.Stop();
        //     // _ball.Position = _ballInitPos;
        //     // _ball.Rotation = _ballInitRot;

        //     // _goalKeeper1.FSM.ChangeState<IdleMainState>();
        //     // _goalKeeper2.FSM.ChangeState<IdleMainState>();

        //     // _ball.gameObject.SetActive(true);
        //     // _goal.GoalTrigger.gameObject.SetActive(true);
        //     // _goal2.GoalTrigger.gameObject.SetActive(true);
            
        // }
    
    }
}
    

