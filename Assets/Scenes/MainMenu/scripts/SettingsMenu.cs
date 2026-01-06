// Import necessary Unity and system namespaces
using NUnit.Framework; // Note: NUnit.Framework is typically for unit testing. If not used for tests, it might be removable.
using System.Collections.Generic; // Required for using List<T>
using UnityEngine; // Core Unity engine functionalities
using UnityEngine.Audio; // For controlling AudioMixer assets
using UnityEngine.UI; // For standard UI elements (though TMP_Dropdown is used here)
using TMPro; // For TextMeshPro UI elements, specifically TMP_Dropdown

// Defines the SettingsMenu class, which must be attached to a GameObject in Unity
public class SettingsMenu : MonoBehaviour
{
    // Public variable to link the AudioMixer from the Unity Inspector.
    // This allows the script to control parameters exposed on the AudioMixer (e.g., master volume).
    public AudioMixer audioMixer;

    // Public variable to link the TextMeshPro Dropdown UI element from the Unity Inspector.
    // This dropdown will be used to display and select screen resolutions.
    public TMP_Dropdown resolutionDropdown;

    // Private array to store all available screen resolutions detected by Unity.
    Resolution[] resolutions;

    // Start is called once before the first frame update when the script instance is being loaded.
    // It's commonly used for initialization tasks.
    private void Start()
    {
        // Retrieve all unique screen resolutions supported by the current display.
        resolutions = Screen.resolutions;

        // Clear any pre-existing options from the resolution dropdown.
        // This ensures the dropdown is populated cleanly each time, especially if it had placeholder values.
        resolutionDropdown.ClearOptions();

        // Create a new list to hold the string representations of the resolution options.
        List<string> options = new List<string>();

        // Initialize an index to keep track of the current screen resolution in the `resolutions` array.
        int currentResolutionIndex = 0;

        // Iterate through each available resolution.
        for (int i = 0; i < resolutions.Length; i++)
        {
            // Format the resolution option string to include width, height, and refresh rate.
            // Example: "1920 x 1080 @ 60hz"
            // Note: resolutions[i].refreshRateRatio might be a new struct in modern Unity or a custom extension.
            // Standard Unity `Resolution` has `refreshRate`. If `refreshRateRatio` is from `Resolution.refreshRateRatio` (Unity 2022.2+),
            // it's a RefreshRate struct. You'd typically use `resolutions[i].refreshRateRatio.value`.
            // Assuming `resolutions[i].refreshRateRatio` directly gives a usable numeric value for display here.
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "hz";
            options.Add(option);

            // Check if the current resolution in the loop matches the game's current screen width and height.
            // Using Screen.width and Screen.height gives the current rendering dimensions of the game window/screen.
            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                // If a match is found, store the index of this resolution.
                // This will be used to set the dropdown to show the current resolution by default.
                currentResolutionIndex = i;
            }
        }

        // Add all the formatted resolution strings to the dropdown's list of options.
        resolutionDropdown.AddOptions(options);

        // Set the dropdown's currently selected value to the index of the current screen resolution.
        resolutionDropdown.value = currentResolutionIndex;

        // Update the dropdown's displayed text to reflect the currently selected value.
        resolutionDropdown.RefreshShownValue();
    }

    // Public method to be called when a new resolution is selected from the dropdown.
    // The `resolutionIndex` parameter is automatically provided by the Dropdown's OnValueChanged event.
    public void SetResolution(int resolutionIndex)
    {
        // Get the Resolution struct from the `resolutions` array using the selected index.
        Resolution resolution = resolutions[resolutionIndex];

        // Apply the selected resolution settings (width, height) and maintain the current fullscreen mode.
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Public method to be called by a UI Slider (or other control) to adjust the game volume.
    public void SetVolume(float volume)
    {
        // Set the exposed "volume" parameter on the linked AudioMixer.
        // The formula Mathf.Log10(volume) * 20 converts a linear slider value (typically 0.0 to 1.0)
        // to a decibel (dB) scale, which is how AudioMixers usually handle volume.
        // A volume of 0 would result in -infinity dB, so ensure the input `volume` is > 0 (e.g., 0.0001f to 1.0f).
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }

    // Public method to be called by a UI Dropdown (or other control) to change the graphics quality level.
    // The `qualityIndex` corresponds to the indices in Unity's Quality Settings (Edit > Project Settings > Quality).
    public void SetQuality(int qualityIndex)
    {
        // Apply the selected quality level. Unity will handle the specific changes associated with that level.
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Public method to be called by a UI Toggle to enable or disable fullscreen mode.
    // The `isFullscreen` parameter is true if the toggle is checked (requesting fullscreen), false otherwise.
    public void SetFullscreen(bool isFullscreen)
    {
        // Set the game's fullscreen state.
        Screen.fullScreen = isFullscreen;
    }
}
