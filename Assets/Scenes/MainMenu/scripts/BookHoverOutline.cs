using UnityEngine;
using System.Collections.Generic;

public class BookHover : MonoBehaviour
{
    [Header("Settings")]
    public Material outlineMaterial; // Drag your 'OutlineMat' here in Inspector

    private MeshRenderer meshRenderer;
    private Material[] originalMaterials; // Stores the clean state of the book

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogError("ERROR: No MeshRenderer found on " + gameObject.name);
            return;
        }

        // Cache the original materials list so we can always revert to it
        originalMaterials = meshRenderer.sharedMaterials;
    }

    void OnMouseEnter()
    {
        Debug.Log("Mouse Enter: " + gameObject.name); // Check Console for this!

        if (outlineMaterial == null)
        {
            Debug.LogError("ERROR: You forgot to assign the Outline Material in the Inspector!");
            return;
        }

        // Create a new list based on the original materials
        List<Material> newMaterials = new List<Material>(originalMaterials);

        // Add the outline material to the end
        newMaterials.Add(outlineMaterial);

        // Apply the new list to the renderer
        meshRenderer.materials = newMaterials.ToArray();
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse Exit: " + gameObject.name);

        // Revert to the clean original list
        if (meshRenderer != null)
        {
            meshRenderer.materials = originalMaterials;
        }
    }
}