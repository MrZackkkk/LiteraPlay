using UnityEngine; // Core Unity functionalities
using UnityEngine.EventSystems;

// Component to handle button interactions and trigger related actions
public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    [Header("Settings")]
    // Reference to the Text GameObject to be moved
    [SerializeField] private GameObject TextObject;
    // Reference to a RectTransform, likely for scaling calculations
    [SerializeField] private RectTransform TargetRectTransform;
    // Reference to a MenuFade script for UI transitions
    [SerializeField] private MenuFade MenuFade;
    // Reference to a CameraMove script for camera positioning
    [SerializeField] private CameraMove CameraMove;
    // Reference to the Camera GameObject
    [SerializeField] private GameObject CameraObject;


    // Stores the initial Y scale of the TargetRectTransform
    private float scaleY;
    // Stores the original position of the Camera GameObject
    private Vector3 originalPositionCamera;
    private bool isHovering = false;
    private bool isPressed = false;

    // Called when the script instance is being loaded
    void Awake()
    {
        // Get the initial Y scale
        scaleY = TargetRectTransform.localScale.y;
        // Store the camera's original position
        originalPositionCamera = CameraObject.transform.position;
    }

    // Quits the application
    public void DoExitGame()
    {
        Application.Quit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        MoveTextDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        MoveTextUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        MoveTextUp();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (eventData.pointerPress == gameObject)
        {
            MoveTextDown();
        }
    }

    // Moves the TextObject down
    public void MoveTextDown()
    {
        if (TextObject != null)
        {
            TextObject.transform.position = new Vector3(
                TextObject.transform.position.x,
                TextObject.transform.position.y - 20 * scaleY,
                TextObject.transform.position.z
        );
        }
    }

    // Moves the TextObject up
    public void MoveTextUp()
    {
        if (TextObject != null)
        {
            TextObject.transform.position = new Vector3(
            TextObject.transform.position.x,
            TextObject.transform.position.y + 20 * scaleY,
            TextObject.transform.position.z

        );
        }
    }

    private void OnDisable()
    {
        isPressed = false;
        MoveTextUp();
    }

    // Handles actions when the Play button is clicked
    public void PlayFunction()
    {
        MenuFade.HideUI("MainMenu"); // Hide the main menu
        MenuFade.ShowUI("BookshelfMenu"); // Show the bookshelf menu
        CameraMove.targetPosition = new Vector3(0f, 0.9f, 2.7f); // Move camera to bookshelf position
    }

    // Handles actions when the Back button is clicked (from bookshelf)
    public void goBackToStart()
    {
        MenuFade.HideUI("BookshelfMenu"); // Hide the bookshelf menu
        MenuFade.ShowUI("MainMenu"); // Show the main menu
        CameraMove.targetPosition = originalPositionCamera; // Move camera back to start
    }

    // Opens the options menu
    public void openOptions()
    {
        MenuFade.ShowUI("OptionsMenu");
    }

    // Closes the options menu
    public void closeOptions()
    {
        MenuFade.HideUI("OptionsMenu");
    }

}
