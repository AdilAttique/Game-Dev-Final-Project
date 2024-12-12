using System;
using Assets.SuperGoalie.Scripts.Managers;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Assets.SuperGoalie.Scripts.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Ball : MonoBehaviour
    {
        [Tooltip("The gravity acting on the ball")]
        public float gravity = 9f;

        public delegate void BallLaunched(float flightTime, float velocity, Vector3 initial, Vector3 target);
        public BallLaunched OnBallLaunched;

        public Rigidbody Rigidbody { get; set; }
        
        public GameObject gameManager;
        private GameManager gameManagerScript;
        public SphereCollider SphereCollider { get; set; }

        private void Awake()
        {
            //get the components
            gameManagerScript = gameManager.GetComponent<GameManager>();
            Rigidbody = GetComponent<Rigidbody>();
            SphereCollider = GetComponent<SphereCollider>();

            // set the gravity of the ball
            Physics.gravity = new Vector3(0f, -gravity, -0f);
        }

        public void Stop()
        {
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.velocity = Vector3.zero;
        }

        public Vector3 FuturePosition(float time)
        {
            //get the velocities
            Vector3 velocity = Rigidbody.velocity;
            Vector3 velocityXZ = velocity;
            velocityXZ.y = 0f;

            //find the future position on the different axis
            float futurePositionY = Position.y + (velocity.y * time + 0.5f * -gravity * Mathf.Pow(time, 2));
            Vector3 futurePositionXZ = Vector3.zero;

            //get the ball future position
            futurePositionXZ = Position + velocityXZ.normalized * velocityXZ.magnitude * time;

            //bundle the future positions to together
            Vector3 futurePosition = futurePositionXZ;
            futurePosition.y = futurePositionY;

            //return the future position
            return futurePosition;
        }

        public void Launch(float power, Vector3 final)
        {
            //set the initial position
            Vector3 initial = Position;

            //find the direction vectors
            Vector3 toTarget = final - initial;
            Vector3 toTargetXZ = toTarget;
            toTargetXZ.y = 0;

            //find the time to target
            float time = toTargetXZ.magnitude / power;

            // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
            // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
            // so xz = v0xz * t => v0xz = xz / t
            // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
            toTargetXZ = toTargetXZ.normalized * toTargetXZ.magnitude / time;

            //set the y-velocity
            Vector3 velocity = toTargetXZ;
            velocity.y = toTarget.y / time + (0.5f * gravity * time);

            //return the velocity
            Rigidbody.velocity = velocity;

            //invoke the ball launched event
            BallLaunched temp = OnBallLaunched;
            if (temp != null)
                temp.Invoke(time, power, initial, final);
        }

        public void Instance_OnBallLaunch(float power, Vector3 target)
        {
            //launch the ball
            Launch(power, target);
        }

        public Quaternion Rotation
        {
            get
            {
                return transform.rotation;
            }

            set
            {
                transform.rotation = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
            }
        }

       private void OnCollisionEnter(Collision other) 
{
    // Check if the colliding GameObject starts with "ThirdPersonController"
    if (other.gameObject.name.StartsWith("ThirdPersonController"))
    {
        // Step 1: Update GameManager variables
        gameManagerScript.hasPossesion = true;
        if(gameManagerScript.player != null)
        if(gameManagerScript.player.CompareTag("Teammate"))
        {
            gameManagerScript.player.GetComponent<ThirdPersonUserControl>().enabled = false;
            gameManagerScript.player.GetComponent<FieldPlayerAI>().enabled = true;
        }
        gameManagerScript.player = other.gameObject;
        gameManagerScript.animator = gameManagerScript.player.GetComponent<Animator>();
        // Step 2: Find the target GameObject ("CameraFollower")
        GameObject targetObject = FindGameObjectInScene("CameraFollower");
        if (targetObject == null)
        {
            Debug.LogError("Target object 'CameraFollower' not found in the scene!");
            return;
        }
        gameManagerScript.player.GetComponent<ThirdPersonUserControl>().enabled = true;
        gameManagerScript.player.GetComponent<FieldPlayerAI>().enabled = false;
        // Step 3: Deparent the target GameObject
        targetObject.transform.SetParent(null);
        
        // Step 4: Reparent the target object to the new parent (player)
        Transform newParent = gameManagerScript.player.transform; // Assign the new parent
        if (newParent != null)
        {
            targetObject.transform.SetParent(newParent);
            
            // Step 5: Set the relative local position
            Vector3 relativePosition = new Vector3(0f, 2.3f, -4.46f);
            targetObject.transform.localPosition = relativePosition;
            Quaternion relativeRotation = Quaternion.Euler(10f, 0f, 0f); // Example rotation (10 degrees on X-axis)
            targetObject.transform.localRotation = relativeRotation;
            
            Debug.Log($"Successfully reparented 'CameraFollower' to {newParent.name} with relative position {relativePosition}.");
        }
        else
        {
            Debug.LogError("New parent (gameManagerScript.player) is null!");
        }
    }

    if(other.gameObject.name.StartsWith("Opponent"))
    {
        Debug.Log("POSSESION TO OPPONENT");
        Debug.Log(other.gameObject.name);
        gameManagerScript.player = other.gameObject;
        gameManagerScript.animator = gameManagerScript.player.GetComponent<Animator>();

            // Vector3 forwardOffset = gameManagerScript.player.transform.forward * 0.8f; // 0.8 units in the forward direction
            // Vector3 targetPosition = gameManagerScript.player.transform.position + forwardOffset;

            // // Update the ball's position
            // transform.position = targetPosition;

    }
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
    }
}
