// using System.Collections.Generic;
// using UnityEngine;

// public class BallController : MonoBehaviour
// {
//     public LineRenderer trajectoryLine; // Line Renderer to visualize the path
//     public Rigidbody ballRigidbody;    // Rigidbody of the ball
//     public float ballSpeed = 5f;       // Speed of the ball movement

//     private List<Vector3> pathPoints = new List<Vector3>(); // List to store the path points
//     private bool isDrawing = false;    // Whether the player is drawing a path
//     private bool isBallMoving = false; // Whether the ball is moving along the path
//     private int currentPointIndex = 0; // Index of the current point in the path

//     void Update()
//     {
//         // Start drawing the path
//         if (Input.GetMouseButtonDown(0) && !isBallMoving)
//         {
//             isDrawing = true;
//             pathPoints.Clear(); // Clear previous path
//             trajectoryLine.positionCount = 0; // Reset Line Renderer
//         }

//         // Add points to the path while dragging
//         if (Input.GetMouseButton(0) && isDrawing)
//         {
//             Vector3 mousePos = Input.mousePosition;
//             mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z; // Maintain depth
//             Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

//             // Add points if far enough from the last point
//             if (pathPoints.Count == 0 || Vector3.Distance(worldPos, pathPoints[pathPoints.Count - 1]) > 0.1f)
//             {
//                 pathPoints.Add(worldPos);
//                 trajectoryLine.positionCount = pathPoints.Count;
//                 trajectoryLine.SetPosition(pathPoints.Count - 1, worldPos);
//             }
//         }

//         // Finish drawing and prepare the ball to move
//         if (Input.GetMouseButtonUp(0) && isDrawing)
//         {
//             isDrawing = false;
//             if (pathPoints.Count > 1) // Ensure there's a valid path
//             {
//                 isBallMoving = true;
//                 currentPointIndex = 0;
//                 ballRigidbody.isKinematic = true; // Disable physics for smooth movement
//             }
//         }

//         // Move the ball along the path
//         if (isBallMoving)
//         {
//             MoveBallAlongPath();
//         }
//     }

//     void MoveBallAlongPath()
//     {
//         if (currentPointIndex < pathPoints.Count)
//         {
//             Vector3 targetPoint = pathPoints[currentPointIndex];
//             transform.position = Vector3.MoveTowards(transform.position, targetPoint, ballSpeed * Time.deltaTime);

//             // Check if the ball has reached the current point
//             if (Vector3.Distance(transform.position, targetPoint) < 0.05f)
//             {
//                 currentPointIndex++;
//             }
//         }
//         else
//         {
//             // Finish moving
//             isBallMoving = false;
//             ballRigidbody.isKinematic = false; // Re-enable physics
//         }
//     }
// }




// using System.Collections.Generic;
// using UnityEngine;

// public class BallController : MonoBehaviour
// {
//     public LineRenderer trajectoryLine; // Line Renderer to visualize the path
//     public Rigidbody ballRigidbody;    // Rigidbody of the ball
//     public float maxForce = 20f;       // Maximum force that can be applied to the ball
//     public float minDistanceToBall = 1f; // Minimum distance from the ball to start the path
//     public LayerMask groundLayer;      // Layer mask for detecting ground

//     private List<Vector3> pathPoints = new List<Vector3>(); // List to store the path points
//     private bool isDrawing = false;    // Whether the player is drawing a path

//     void Update()
//     {
//         HandleDrawingPath();
//         HandleForceApplication();
//     }

//     void HandleDrawingPath()
//     {
//         // Start drawing the path
//         if (Input.GetMouseButtonDown(0))
//         {
//             Vector3 mouseWorldPosition = GetMouseWorldPosition();

//             if (Vector3.Distance(mouseWorldPosition, transform.position) <= minDistanceToBall)
//             {
//                 isDrawing = true;
//                 pathPoints.Clear(); // Clear previous path
//                 trajectoryLine.positionCount = 0; // Reset Line Renderer
//             }
//         }

//         // Add points to the path while dragging
//         if (Input.GetMouseButton(0) && isDrawing)
//         {
//             Vector3 mouseWorldPosition = GetMouseWorldPosition();

//             if (pathPoints.Count == 0 || Vector3.Distance(mouseWorldPosition, pathPoints[pathPoints.Count - 1]) > 0.2f)
//             {
//                 pathPoints.Add(mouseWorldPosition);
//                 trajectoryLine.positionCount = pathPoints.Count;
//                 trajectoryLine.SetPosition(pathPoints.Count - 1, mouseWorldPosition);
//             }
//         }

//         // Stop drawing
//         if (Input.GetMouseButtonUp(0) && isDrawing)
//         {
//             isDrawing = false;
//         }
//     }

//     void HandleForceApplication()
//     {
//         if (!isDrawing && pathPoints.Count > 1)
//         {
//             if (ValidateTrajectory(pathPoints))
//             {
//                 ApplyForceFromPath(pathPoints);
//             }
//             else
//             {
//                 Debug.Log("Invalid trajectory. Please try again.");
//             }

//             pathPoints.Clear(); // Clear the path after applying force
//             trajectoryLine.positionCount = 0; // Clear the trajectory line
//         }
//     }

//     Vector3 GetMouseWorldPosition()
//     {
//         Vector3 mousePos = Input.mousePosition;
//         mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
//         return Camera.main.ScreenToWorldPoint(mousePos);
//     }

//     bool ValidateTrajectory(List<Vector3> path)
//     {
//         if (path.Count < 2) return false;

//         // Ensure trajectory moves forward and not downward excessively
//         for (int i = 1; i < path.Count; i++)
//         {
//             if (path[i].y < path[i - 1].y - 0.5f) // Disallow steep downward slopes
//                 return false;
//         }

//         return true;
//     }

//     void ApplyForceFromPath(List<Vector3> path)
//     {
//         Vector3 initialDirection = (path[1] - path[0]).normalized; // Direction of initial force
//         float pathLength = 0f;

//         for (int i = 1; i < path.Count; i++)
//         {
//             pathLength += Vector3.Distance(path[i], path[i - 1]);
//         }

//         float forceMagnitude = Mathf.Clamp(pathLength * 2f, 0, maxForce);
//         Vector3 force = initialDirection * forceMagnitude;

//         ballRigidbody.isKinematic = false; // Enable physics
//         ballRigidbody.velocity = Vector3.zero; // Reset velocity
//         ballRigidbody.AddForce(force, ForceMode.Impulse); // Apply force

//         Debug.Log($"Applied force: {force} (magnitude: {forceMagnitude})");
//     }
// }




// using System.Collections.Generic;
// using UnityEngine;

// public class BallController : MonoBehaviour
// {
//     public LineRenderer trajectoryLine; // Line Renderer to visualize the path
//     public Rigidbody ballRigidbody;    // Rigidbody of the ball
//     public float maxForce = 20f;       // Maximum force that can be applied to the ball
//     public float curveAdjustmentStrength = 1f; // Strength of the curve correction
//     public float minDistanceToBall = 1f; // Minimum distance from the ball to start the path
//     public LayerMask groundLayer;      // Layer mask for detecting ground
//     public float maxRealisticCurveDistance = 2f; // Maximum realistic deviation for the curve

//     private List<Vector3> pathPoints = new List<Vector3>(); // List to store the path points
//     private bool isDrawing = false;    // Whether the player is drawing a path
//     private bool isBallMoving = false; // Whether the ball is currently following the trajectory
//     private int currentPointIndex = 0; // Current target point index in the trajectory

//     void Update()
//     {
//         HandleDrawingPath();
//     }

//     void FixedUpdate()
//     {
//         if (isBallMoving)
//         {
//             ApplyCurvingForce();
//         }
        
//     }

//     void HandleDrawingPath()
//     {
//         // Start drawing the path
//         if (Input.GetMouseButtonDown(0))
//         {
//             Vector3 mouseWorldPosition = GetMouseWorldPosition();

//             if (Vector3.Distance(mouseWorldPosition, transform.position) <= minDistanceToBall)
//             {
//                 isDrawing = true;
//                 pathPoints.Clear(); // Clear previous path
//                 trajectoryLine.positionCount = 0; // Reset Line Renderer
//             }
//         }

//         // Add points to the path while dragging
//         if (Input.GetMouseButton(0) && isDrawing)
//         {
//             Vector3 mouseWorldPosition = GetMouseWorldPosition();

//             if (pathPoints.Count == 0 || Vector3.Distance(mouseWorldPosition, pathPoints[pathPoints.Count - 1]) > 0.2f)
//             {
//                 pathPoints.Add(mouseWorldPosition);
//                 trajectoryLine.positionCount = pathPoints.Count;
//                 trajectoryLine.SetPosition(pathPoints.Count - 1, mouseWorldPosition);
//             }
//         }

//         // Stop drawing
//         if (Input.GetMouseButtonUp(0) && isDrawing)
//         {
//             isDrawing = false;
//             if (pathPoints.Count > 1)
//             {
//                 LaunchBall();
//             }
//         }
//     }

//     void LaunchBall()
//     {
//         Vector3 initialDirection = (pathPoints[1] - pathPoints[0]).normalized;
//         float pathLength = 0f;

//         // Calculate the total path length
//         for (int i = 1; i < pathPoints.Count; i++)
//         {
//             pathLength += Vector3.Distance(pathPoints[i], pathPoints[i - 1]);
//         }

//         float forceMagnitude = Mathf.Clamp(pathLength * 2f, 0, maxForce);
//         Vector3 initialForce = initialDirection * forceMagnitude;

//         ballRigidbody.isKinematic = false; // Enable physics
//         ballRigidbody.velocity = Vector3.zero; // Reset velocity
//         ballRigidbody.AddForce(initialForce, ForceMode.Impulse); // Apply initial force

//         isBallMoving = true; // Begin trajectory adjustment
//         currentPointIndex = 1; // Start following the path
//     }

//     void ApplyCurvingForce()
//     {
//         if (currentPointIndex >= pathPoints.Count) return;

//         // Find the closest point in the trajectory to the ball
//         Vector3 currentTarget = pathPoints[currentPointIndex];
//         Vector3 ballPosition = transform.position;

//         // Adjust the velocity slightly towards the current path point
//         Vector3 toTarget = currentTarget - ballPosition;
//         Vector3 adjustment = toTarget.normalized * curveAdjustmentStrength * Time.fixedDeltaTime;

//         // Limit adjustment strength to maintain realistic curves
//         if (adjustment.magnitude > maxRealisticCurveDistance)
//         {
//             adjustment = adjustment.normalized * maxRealisticCurveDistance;
//         }

//         ballRigidbody.velocity += adjustment;

//         // Check if the ball is close enough to the current target to move to the next point
//         if (toTarget.magnitude < 0.5f && currentPointIndex < pathPoints.Count - 1)
//         {
//             currentPointIndex++;
//         }

//         // Stop curving adjustment when the ball exits the trajectory
//         if (currentPointIndex >= pathPoints.Count)
//         {
//             isBallMoving = false;
//         }
//     }

//     Vector3 GetMouseWorldPosition()
//     {
//         Vector3 mousePos = Input.mousePosition;
//         mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z;
//         return Camera.main.ScreenToWorldPoint(mousePos);
//     }
    
// }




// using UnityEngine;
// using System.Collections;
// public class BallMovement : MonoBehaviour
// {
//     public LineRenderer trajectoryLine; // LineRenderer to visualize the trajectory
//     public float maxForce = 30f;       // Maximum force for the ball
//     public float curveIntensity = 5f; // Intensity of the ball curve
//     public AnimationCurve curveProfile; // Curve profile for fine control

//     private Rigidbody ballRigidbody;
//     private Vector3 initialPosition;
//     private Vector3 releasePosition;
//     private bool isDragging = false;

//     void Start()
//     {
//         ballRigidbody = GetComponent<Rigidbody>();
//         if (trajectoryLine != null)
//             trajectoryLine.positionCount = 0; // Ensure the trajectory starts empty
//     }

//     void Update()
//     {
//         HandleInput();
//     }

//     private void HandleInput()
//     {
//         if (Input.GetMouseButtonDown(0)) // Start dragging
//         {
//             initialPosition = GetMouseWorldPosition();
//             isDragging = true;
//         }

//         if (Input.GetMouseButton(0) && isDragging) // Update trajectory
//         {
//             releasePosition = GetMouseWorldPosition();
//             DrawTrajectory(initialPosition, releasePosition);
//         }

//         if (Input.GetMouseButtonUp(0) && isDragging) // Release and shoot
//         {
//             isDragging = false;
//             trajectoryLine.positionCount = 0; // Clear the trajectory
//             ShootBall(initialPosition, releasePosition);
//         }
//     }

//     private Vector3 GetMouseWorldPosition()
//     {
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         if (Physics.Raycast(ray, out RaycastHit hit))
//         {
//             return hit.point;
//         }
//         return Vector3.zero;
//     }

//     private void DrawTrajectory(Vector3 start, Vector3 end)
//     {
//         Vector3 direction = end - start;
//         float force = Mathf.Clamp(direction.magnitude, 0, maxForce);

//         Vector3 initialVelocity = direction.normalized * force;
//         float simulationTimeStep = 0.1f;
//         float simulationTime = 0f;

//         trajectoryLine.positionCount = 30; // Set the number of trajectory points
//         for (int i = 0; i < trajectoryLine.positionCount; i++)
//         {
//             Vector3 simulatedPosition = start + 
//                 initialVelocity * simulationTime + 
//                 0.5f * Physics.gravity * simulationTime * simulationTime;
//             trajectoryLine.SetPosition(i, simulatedPosition);
//             simulationTime += simulationTimeStep;
//         }
//     }

//     private void ShootBall(Vector3 start, Vector3 end)
//     {
//         Vector3 direction = end - start;
//         float force = Mathf.Clamp(direction.magnitude, 0, maxForce);
//         Vector3 initialVelocity = direction.normalized * force;

//         ballRigidbody.velocity = initialVelocity;

//         // Apply curve
//         StartCoroutine(ApplyCurve(initialVelocity));
//     }

//     private IEnumerator ApplyCurve(Vector3 initialVelocity)
//     {
//         float elapsedTime = 0f;
//         while (elapsedTime < 2f) // Simulate curve for 2 seconds
//         {
//             elapsedTime += Time.deltaTime;

//             // Add curve force perpendicular to the direction
//             Vector3 perpendicularForce = Vector3.Cross(initialVelocity, Vector3.up).normalized * curveIntensity;
//             perpendicularForce *= curveProfile.Evaluate(elapsedTime / 2f); // Use the curve profile

//             ballRigidbody.AddForce(perpendicularForce, ForceMode.Acceleration);
//             yield return null;
//         }
//     }
// }



using System.Collections;
using UnityEngine;
using UnityStandardAssets.Vehicles.Ball;

[RequireComponent(typeof(LineRenderer))]
public class BallScript : MonoBehaviour
{
    public float shootingForceMultiplier = 10f; // Multiplier for shooting force
    public int trajectoryPoints = 50; // Number of points for trajectory visualization
    public float trajectoryTimeStep = 0.1f; // Time step between trajectory points
    public GameObject playerFeet;
    public bool hasBall = true;

    private Rigidbody rb;
    private LineRenderer lineRenderer;
    private bool isDragging = false;
    private Vector3 startDragPosition;
    private Vector3 endDragPosition;

    private Animator animator;
    public GameObject player;

    void Start()
    {
        animator = player.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0; // Clear trajectory at start
    }

    void Update()
    {
        if(hasBall == true)
        {
            Vector3 temp = playerFeet.transform.position;
            temp.z += 0.3f;
            temp.y += 0.1f;
            gameObject.transform.position = temp;
        }
            
        HandleInput();
    }

    private void HandleInput()
    {
        // Begin drag
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startDragPosition = GetMouseWorldPosition();
        }

        // Dragging
        if (Input.GetMouseButton(0) && isDragging)
        {
            endDragPosition = GetMouseWorldPosition();
            Vector3 forceDirection = (endDragPosition - startDragPosition).normalized;
            float forceMagnitude = (endDragPosition - startDragPosition).magnitude * shootingForceMultiplier;

            // Update trajectory visualization
            DrawTrajectory(transform.position, forceDirection * forceMagnitude);
        }

        // Release drag to shoot
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            animator.Play("Pass");
            hasBall = false;
            ShootBall();
            ClearTrajectory();
        }
    }

    private void ShootBall()
    {
        Vector3 force = (endDragPosition - startDragPosition).normalized *
                        (endDragPosition - startDragPosition).magnitude * shootingForceMultiplier;
        rb.velocity = force;
    }

    private void DrawTrajectory(Vector3 startPosition, Vector3 initialVelocity)
    {
        lineRenderer.positionCount = trajectoryPoints;
        Vector3 currentPosition = startPosition;
        Vector3 currentVelocity = initialVelocity;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            lineRenderer.SetPosition(i, currentPosition);

            // Update position and velocity for the next point
            currentPosition += currentVelocity * trajectoryTimeStep;
            currentVelocity += Physics.gravity * trajectoryTimeStep; // Incorporate gravity
        }
    }

    private void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.name.Equals("ThirdPersonController"))
        {
            hasBall = true;
        }
    }
}



// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(LineRenderer), typeof(Rigidbody))]
// public class BallController : MonoBehaviour
// {
//     public GameObject playerFeet;

//     private Animator animator;
//     public GameObject player;    
//     public bool hasBall = true;
//     public LineRenderer trajectoryLine; // Line Renderer for trajectory visualization
//     public Rigidbody ballRigidbody;    // Ball Rigidbody
//     public float maxForce = 20f;       // Maximum launch force
//     public float pointSpacing = 0.2f;  // Minimum distance between trajectory points
//     public float curveSmoothness = 0.1f; // Smoothness factor for trajectory interpolation
//     public LayerMask groundLayer;      // Ground layer for raycast

//     private List<Vector3> pathPoints = new List<Vector3>(); // List of trajectory points
//     private bool isDrawing = false;    // Is the user currently drawing?
//     private bool isBallMoving = false; // Is the ball following the trajectory?
//     private int currentPointIndex = 0; // Current target point in the path

//     void Start()
//     {
//         trajectoryLine.positionCount = 0; // Initialize Line Renderer
//         ballRigidbody.isKinematic = true; // Disable physics at the start
//     }

//     void Update()
//     {
//         HandlePathDrawing();
//     }

//     void FixedUpdate()
//     {
//         if (isBallMoving)
//         {
//             FollowTrajectory();
//         }
//     }

//     void HandlePathDrawing()
//     {
//         if (Input.GetMouseButtonDown(0)) // Start drawing
//         {
//             Vector3 mousePosition = GetMouseWorldPosition();
//             if (Vector3.Distance(mousePosition, transform.position) <= 1f)
//             {
//                 isDrawing = true;
//                 pathPoints.Clear(); // Clear previous path
//                 trajectoryLine.positionCount = 0; // Reset line renderer
//             }
//         }

//         if (Input.GetMouseButton(0) && isDrawing) // Continue drawing
//         {
//             Vector3 mousePosition = GetMouseWorldPosition();
//             if (pathPoints.Count == 0 || Vector3.Distance(mousePosition, pathPoints[^1]) >= pointSpacing)
//             {
//                 pathPoints.Add(mousePosition);
//                 trajectoryLine.positionCount = pathPoints.Count;
//                 trajectoryLine.SetPosition(pathPoints.Count - 1, mousePosition);
//             }
//         }

//         if (Input.GetMouseButtonUp(0) && isDrawing) // Stop drawing
//         {
//             isDrawing = false;
//             animator.Play("Pass");
//             hasBall = false;
//             if (pathPoints.Count > 1)
//             {
//                 LaunchBall();
//             }
//         }
//     }

//     void LaunchBall()
//     {
//         // Interpolate the path to smooth out sharp edges
//         pathPoints = SmoothPath(pathPoints);

//         // Apply force in the direction of the first segment
//         Vector3 initialDirection = (pathPoints[1] - pathPoints[0]).normalized;
//         float pathLength = CalculatePathLength(pathPoints);
//         float forceMagnitude = Mathf.Clamp(pathLength, 0, maxForce);

//         Vector3 initialForce = initialDirection * forceMagnitude;

//         // Add loft based on the average height difference in the trajectory
//         float averageHeight = CalculateAverageHeight(pathPoints);
//         initialForce.y += averageHeight * 0.5f; // Adjust height multiplier for natural loft

//         ballRigidbody.isKinematic = false; // Enable physics
//         ballRigidbody.velocity = Vector3.zero; // Reset velocity
//         ballRigidbody.AddForce(initialForce, ForceMode.Impulse); // Apply force

//         isBallMoving = true; // Begin trajectory following
//         currentPointIndex = 1;
//     }

//     void FollowTrajectory()
//     {
//         if (currentPointIndex >= pathPoints.Count)
//         {
//             isBallMoving = false;
//             return;
//         }

//         Vector3 targetPoint = pathPoints[currentPointIndex];
//         Vector3 currentPosition = transform.position;
//         Vector3 toTarget = targetPoint - currentPosition;

//         // Adjust velocity to move towards the target point
//         ballRigidbody.velocity = Vector3.Lerp(ballRigidbody.velocity, toTarget.normalized * ballRigidbody.velocity.magnitude, Time.fixedDeltaTime * 5f);

//         if (toTarget.magnitude < 0.5f)
//         {
//             currentPointIndex++;
//         }
//     }

//     Vector3 GetMouseWorldPosition()
//     {
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
//         {
//             return hit.point;
//         }
//         return Vector3.zero;
//     }

//     List<Vector3> SmoothPath(List<Vector3> points)
//     {
//         List<Vector3> smoothedPath = new List<Vector3>();

//         for (int i = 0; i < points.Count - 1; i++)
//         {
//             smoothedPath.Add(points[i]);
//             Vector3 midPoint = Vector3.Lerp(points[i], points[i + 1], 0.5f);
//             smoothedPath.Add(midPoint);
//         }

//         smoothedPath.Add(points[^1]);
//         return smoothedPath;
//     }

//     float CalculatePathLength(List<Vector3> points)
//     {
//         float length = 0f;
//         for (int i = 1; i < points.Count; i++)
//         {
//             length += Vector3.Distance(points[i], points[i - 1]);
//         }
//         return length;
//     }

//     float CalculateAverageHeight(List<Vector3> points)
//     {
//         float totalHeight = 0f;
//         foreach (Vector3 point in points)
//         {
//             totalHeight += point.y;
//         }
//         return totalHeight / points.Count;
//     }

//     private void OnCollisionEnter(Collision other) {
//         if(other.gameObject.name.Equals("ThirdPersonController"))
//         {
//             hasBall = true;
//         }
//     }
// }

