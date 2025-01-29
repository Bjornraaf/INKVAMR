using UnityEngine;
using UnityEngine.UI;

public class ChangeCarMaterial : MonoBehaviour
{
    public Material[] normalMaterials;  // List for normal materials for the car body
    public Material[] carbonMaterials;  // List for carbon materials for the car body
    public Material[] tireMaterials;  // List for tire materials

    public GameObject carBody;  // Reference to the car body GameObject
    public GameObject[] tires;  // References to the tire GameObjects

    private Renderer bodyRenderer;  // Renderer of the car body
    private Renderer[] tireRenderers;  // Renderers of the tires

    public int currentBodyMaterialIndex = 0;  // Index to keep track of the current body material
    public int currentTireMaterialIndex = 0;  // Index to keep track of the current tire material
    public bool useCarbonColors = false;  // Boolean to switch between normal and carbon materials

    public Toggle switchGroupsToggle; // Reference to the toggle in the UI

    void Start()
    {
        // Initialize body renderer
        if (carBody != null)
        {
            bodyRenderer = carBody.GetComponent<Renderer>();
        }

        // Initialize tire renderers
        tireRenderers = new Renderer[tires.Length];
        for (int i = 0; i < tires.Length; i++)
        {
            if (tires[i] != null)
            {
                tireRenderers[i] = tires[i].GetComponent<Renderer>();
            }
        }

        // Ensure the toggle reflects the current state of 'useCarbonColors'
        if (switchGroupsToggle != null)
        {
            switchGroupsToggle.isOn = useCarbonColors;
        }
    }

    // Function to change to the next body material
    public void NextBodyMaterial()
    {
        if (bodyRenderer != null)
        {
            currentBodyMaterialIndex = (currentBodyMaterialIndex + 1) % (useCarbonColors ? carbonMaterials.Length : normalMaterials.Length);
            bodyRenderer.material = useCarbonColors ? carbonMaterials[currentBodyMaterialIndex] : normalMaterials[currentBodyMaterialIndex];
        }
    }

    // Function to change to the previous body material
    public void PreviousBodyMaterial()
    {
        if (bodyRenderer != null)
        {
            currentBodyMaterialIndex = (currentBodyMaterialIndex - 1 + (useCarbonColors ? carbonMaterials.Length : normalMaterials.Length)) % (useCarbonColors ? carbonMaterials.Length : normalMaterials.Length);
            bodyRenderer.material = useCarbonColors ? carbonMaterials[currentBodyMaterialIndex] : normalMaterials[currentBodyMaterialIndex];
        }
    }

    // Function to switch between normal and carbon materials for the body
    public void SwitchBodyMaterialGroup()
    {
        useCarbonColors = !useCarbonColors;
        currentBodyMaterialIndex = 0;

        // Update the toggle UI to reflect the new state
        if (switchGroupsToggle != null)
        {
            switchGroupsToggle.isOn = useCarbonColors;
        }

        Debug.Log("Switched to " + (useCarbonColors ? "Carbon" : "Normal") + " materials for the body.");
    }

    // Function to change to the next tire material
    public void NextTireMaterial()
    {
        if (tireRenderers.Length > 0)
        {
            currentTireMaterialIndex = (currentTireMaterialIndex + 1) % tireMaterials.Length;
            foreach (var tireRenderer in tireRenderers)
            {
                tireRenderer.material = tireMaterials[currentTireMaterialIndex];
            }
        }
    }

    // Function to change to the previous tire material
    public void PreviousTireMaterial()
    {
        if (tireRenderers.Length > 0)
        {
            currentTireMaterialIndex = (currentTireMaterialIndex - 1 + tireMaterials.Length) % tireMaterials.Length;
            foreach (var tireRenderer in tireRenderers)
            {
                tireRenderer.material = tireMaterials[currentTireMaterialIndex];
            }
        }
    }

    // Function to randomize both the body and tire materials, as well as the toggle
    public void RandomizeMaterials()
    {
        // Randomize the toggle (use normal or carbon materials for the body)
        useCarbonColors = Random.Range(0, 2) == 1;  // Randomly set to true (carbon) or false (normal)
        currentBodyMaterialIndex = 0;  // Reset the body material index after changing the group

        // Update the toggle UI to reflect the randomized state
        if (switchGroupsToggle != null)
        {
            switchGroupsToggle.isOn = useCarbonColors;
        }

        // Randomize body material
        int randomBodyIndex = Random.Range(0, useCarbonColors ? carbonMaterials.Length : normalMaterials.Length);
        bodyRenderer.material = useCarbonColors ? carbonMaterials[randomBodyIndex] : normalMaterials[randomBodyIndex];

        // Randomize tire material
        int randomTireIndex = Random.Range(0, tireMaterials.Length);
        foreach (var tireRenderer in tireRenderers)
        {
            tireRenderer.material = tireMaterials[randomTireIndex];
        }

        // Log the randomized materials and toggle state
        Debug.Log("Materials have been randomized. Body material group: " + (useCarbonColors ? "Carbon" : "Normal"));
    }
}