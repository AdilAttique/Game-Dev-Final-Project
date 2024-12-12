using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The object to follow

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0f, 0f, 0f); // Offset position relative to the target
    public float followSpeed = 10f; // Speed of the camera movement
    public float rotationSpeed = 5f; // Speed of the camera rotation

    [Header("Look Settings")]
    public bool lookAtTarget = true; // Whether the camera should look at the target

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned for the camera to follow!");
            return;
        }

        // Smoothly move the camera to the target position
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Smoothly rotate the camera to follow the target's rotation
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);

        // Optional: Ensure the camera is looking directly at the target
        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}
