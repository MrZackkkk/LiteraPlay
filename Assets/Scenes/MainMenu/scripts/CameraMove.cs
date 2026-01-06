// Import necessary Unity namespaces
using UnityEngine;

// Component to handle camera movement
public class CameraMove : MonoBehaviour
{
    // Reference to the main camera
    [SerializeField] private Camera mainCamera;

    // Smoothness of camera position movement
    public float smoothTime = 1f;

    // Desired target position for the camera
    public Vector3 targetPosition;

    // Velocity reference for SmoothDamp
    private Vector3 velocity = Vector3.zero;

    // Called when the script instance is loaded
    public void Awake()
    {
        // Initialize target position to current camera position
        targetPosition = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
    }

    // Called once per frame
    private void Update()
    {
        // Smoothly move camera towards target position
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);

    }
}
