// Import necessary Unity namespaces
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

// Component to handle camera movement
public class CameraMove : MonoBehaviour
{
    // Reference to the main camera
    [SerializeField] private Camera mainCamera;

    // Target rotation for the camera (currently unused)
    private Quaternion targetRotation;

    // Smoothness of camera position movement
    public float smoothTime = 1f;

    // Reference to a ButtonController (currently unused)
    public ButtonController controller;

    // Flag indicating if the camera is at the start position (currently unused)
    public bool atStart = false;

    // Flag indicating if the camera is at the bookshelf position (currently unused)
    public bool atBookshelf = false;

    // Desired target position for the camera
    public Vector3 targetPosition;

    // Velocity reference for SmoothDamp
    private Vector3 velocity = Vector3.zero;

    // Smoothness of camera rotation (currently unused)
    public float rotationSmoothTime = 1.0f;

    // Threshold for position comparison (currently unused)
    public float positionThreshold = 0.1f;

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

        // Rotation logic could be added here using targetRotation and rotationSmoothTime
    }
}
