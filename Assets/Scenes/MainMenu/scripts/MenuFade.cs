using UnityEngine; // Core Unity functionalities
using UnityEngine.UI; // UI components (like Button, though not directly used here)
using System.Collections.Generic; // For using List and Dictionary

// Component to handle fading UI menus in and out
public class MenuFade : MonoBehaviour
{
    // Serializable class to define a menu with its name and CanvasGroup
    [System.Serializable]
    public class Menu
    {
        public string name; // Name of the menu
        public CanvasGroup canvasGroup; // CanvasGroup for fading control
    }

    // List of menus managed by this script
    public List<Menu> menus;
    // Duration of the fade animation
    [SerializeField] private float fadeDuration = 0.1f;
    [SerializeField] private float transitionThreshold = 0.01f;
    [SerializeField] private bool disableWhenIdle = true;

    private bool isAnyMenuTransitioning;

    // Internal class to track fade information for each menu
    private class FadeInfo
    {
        public CanvasGroup canvasGroup; // Reference to the CanvasGroup
        public float targetAlpha; // Desired alpha value (0 for hidden, 1 for visible)
        public float fadeVelocity; // Velocity used by SmoothDamp
    }

    // Dictionary to easily access fade info by menu name
    private Dictionary<string, FadeInfo> fadeMenus = new Dictionary<string, FadeInfo>();

    // Called when the script starts
    void Start()
    {
        // Initialize all menus to be hidden
        foreach (var menu in menus)
        {
            fadeMenus[menu.name] = new FadeInfo
            {
                canvasGroup = menu.canvasGroup,
                targetAlpha = 0f,
                fadeVelocity = 0f
            };
            menu.canvasGroup.alpha = 0f;
            menu.canvasGroup.interactable = false;
            menu.canvasGroup.blocksRaycasts = false;
            menu.canvasGroup.gameObject.SetActive(false); // Deactivate initially
        }

        // Show the main menu on start
        ShowUI("MainMenu");
    }

    // Method to show a specific UI menu
    public void ShowUI(string menuName)
    {
        if (!fadeMenus.ContainsKey(menuName)) return; // Exit if menu not found

        var targetMenu = fadeMenus[menuName];
        targetMenu.targetAlpha = 1f; // Set target alpha to fully visible
        targetMenu.canvasGroup.gameObject.SetActive(true); // Activate the game object
        if (disableWhenIdle)
            enabled = true;
    }

    // Method to hide a specific UI menu
    public void HideUI(string menuName)
    {
        if (!fadeMenus.ContainsKey(menuName)) return; // Exit if menu not found

        var menu = fadeMenus[menuName];
        menu.targetAlpha = 0f; // Set target alpha to fully hidden
        if (disableWhenIdle)
            enabled = true;
    }

    // Called every frame
    private void Update()
    {
        // Determine smooth time, preventing division by zero
        float smoothTime = fadeDuration > 0 ? fadeDuration : 0.01f;
        bool anyMenuTransitioning = false;

        // Update alpha for all managed menus
        foreach (var menu in fadeMenus.Values)
        {
            float currentAlpha = menu.canvasGroup.alpha;
            float alphaDifference = Mathf.Abs(currentAlpha - menu.targetAlpha);
            float newAlpha = currentAlpha;

            if (alphaDifference > transitionThreshold)
            {
                anyMenuTransitioning = true;
                // Smoothly transition current alpha towards target alpha
                newAlpha = Mathf.SmoothDamp(currentAlpha, menu.targetAlpha, ref menu.fadeVelocity, smoothTime);
            }
            else
            {
                menu.fadeVelocity = 0f;
                newAlpha = menu.targetAlpha;
            }

            menu.canvasGroup.alpha = newAlpha;

            // Handle interactability and raycasting based on target alpha
            if (menu.targetAlpha > 0f)
            {
                // Activate and enable interaction if targeting visible
                if (!menu.canvasGroup.gameObject.activeSelf)
                    menu.canvasGroup.gameObject.SetActive(true);

                menu.canvasGroup.interactable = true;
                menu.canvasGroup.blocksRaycasts = true;
            }
            else if (menu.targetAlpha == 0f && newAlpha <= 0.01f)
            {
                // Deactivate and disable interaction if targeting hidden and nearly invisible
                menu.canvasGroup.alpha = 0f; // Ensure it's exactly 0
                menu.canvasGroup.interactable = false;
                menu.canvasGroup.blocksRaycasts = false;
                menu.canvasGroup.gameObject.SetActive(false);
            }
        }

        isAnyMenuTransitioning = anyMenuTransitioning;
        if (disableWhenIdle && !isAnyMenuTransitioning)
            enabled = false;
    }
}
