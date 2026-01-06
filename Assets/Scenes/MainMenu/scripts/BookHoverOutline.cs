using UnityEngine;
using System.Collections.Generic;

public class BookHover : MonoBehaviour
{
    [Header("Settings")]
    public Material outlineMaterial; // Drag your 'OutlineMat' here in Inspector

    private Renderer[] targetRenderers;
    private Material[][] originalMaterials; // Stores the clean state of the book
    private bool isOutlined;

    void Start()
    {
        targetRenderers = GetComponentsInChildren<Renderer>(true);

        if (targetRenderers == null || targetRenderers.Length == 0)
        {
            Debug.LogError("ERROR: No Renderer found on " + gameObject.name);
            return;
        }

        // Cache the original materials list so we can always revert to it
        originalMaterials = new Material[targetRenderers.Length][];
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            originalMaterials[i] = targetRenderers[i].sharedMaterials;
        }
    }

    void OnMouseEnter()
    {
        Debug.Log("Mouse Enter: " + gameObject.name); // Check Console for this!

        if (outlineMaterial == null)
        {
            Debug.LogError("ERROR: You forgot to assign the Outline Material in the Inspector!");
            return;
        }

        if (isOutlined)
        {
            return;
        }

        for (int i = 0; i < targetRenderers.Length; i++)
        {
            // Create a new list based on the original materials
            List<Material> newMaterials = new List<Material>(originalMaterials[i]);

            // Add the outline material to the end
            if (!newMaterials.Contains(outlineMaterial))
            {
                newMaterials.Add(outlineMaterial);
            }

            // Apply the new list to the renderer
            targetRenderers[i].materials = newMaterials.ToArray();
        }

        isOutlined = true;
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse Exit: " + gameObject.name);

        // Revert to the clean original list
        if (targetRenderers != null)
        {
            for (int i = 0; i < targetRenderers.Length; i++)
            {
                targetRenderers[i].materials = originalMaterials[i];
            }
        }

        isOutlined = false;
    }
}
