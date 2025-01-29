using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button nextBodyMaterialButton;  // Button to go to the next body material
    public Button previousBodyMaterialButton;  // Button to go to the previous body material
    public Button nextTireMaterialButton;  // Button to go to the next tire material
    public Button previousTireMaterialButton;  // Button to go to the previous tire material
    public Button randomizeButton;  // Button to randomize both body and tire materials
    public Toggle switchGroupsToggle;    // Reference to the toggle

    private ChangeCarMaterial carMaterialScript;  // Reference to the ChangeCarMaterial script

    void Update()
    {
        // Check if the script is already found
        if (carMaterialScript == null)
        {
            // Try to find the script in the scene
            carMaterialScript = FindObjectOfType<ChangeCarMaterial>();

            // If found, set the button listeners and toggle listener
            if (carMaterialScript != null)
            {
                // Add listener to next body material button
                nextBodyMaterialButton.onClick.AddListener(carMaterialScript.NextBodyMaterial);

                // Add listener to previous body material button
                previousBodyMaterialButton.onClick.AddListener(carMaterialScript.PreviousBodyMaterial);

                // Add listener to next tire material button
                nextTireMaterialButton.onClick.AddListener(carMaterialScript.NextTireMaterial);

                // Add listener to previous tire material button
                previousTireMaterialButton.onClick.AddListener(carMaterialScript.PreviousTireMaterial);

                // Add listener to switch material groups (for the car body)
                switchGroupsToggle.onValueChanged.AddListener(OnToggleValueChanged);

                // Add listener to the randomize button
                randomizeButton.onClick.AddListener(carMaterialScript.RandomizeMaterials);

                Debug.Log("ChangeCarMaterial script found!");
            }
        }
    }

    // Method to handle toggle value change
    private void OnToggleValueChanged(bool isOn)
    {
        // Update the useCarbonColors boolean based on the toggle value
        carMaterialScript.useCarbonColors = isOn;

        // Optionally reset the material index to start with the first material of the selected group for the body
        carMaterialScript.currentBodyMaterialIndex = 0;
    }
}
