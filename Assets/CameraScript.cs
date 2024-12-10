using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // The player or object to follow

    [Header("Offset")]
    public Vector3 offset = new Vector3(0, 5, -10); // Camera position offset relative to the target

    [Header("Smoothness")]
    [Range(0.01f, 1f)]
    public float smoothSpeed = 0.125f; // Adjust the smoothness of the camera's movement

    [Header("Rotation")]
    public bool followRotation = true; // If true, the camera will rotate with the target
    public float rotationSmoothSpeed = 5f; // Adjust the speed of rotation smoothing

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Smooth position update
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Smooth rotation update (optional)
        if (followRotation)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
        }
    }
}
