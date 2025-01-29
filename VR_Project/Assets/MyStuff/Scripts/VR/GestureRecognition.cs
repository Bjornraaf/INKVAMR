using UnityEngine;
using UnityEngine.XR;

public class GestureRecognition : MonoBehaviour
{
    public XRNode inputSource; // Which controller to use (left or right)
    private InputDevice device;
    private Vector3 previousPosition;

    [SerializeField]
    private float wavingThreshold = 0.2f; // How much movement is considered a wave
    [SerializeField]
    private float pointingThreshold = 0.1f; // Threshold to determine pointing

    public GameObject fireEffectPrefab; // Fire effect prefab to spawn at the controller
    private GameObject fireEffectInstance; // Instance of the fire effect
    private bool isDetectionPaused = false; // Flag to pause detection temporarily
    private float detectionPauseTime = 2f; // Time in seconds to pause the detection after pointing gesture
    private float detectionPauseTimer = 0f; // Timer to track pause duration

    private bool isPointing = false; // Flag to track if pointing is ongoing
    private bool isWaving = false; // Flag to track if waving is ongoing
    private bool canDetectGestures = false; // Flag to track if gestures are enabled by pressing the A button

     [SerializeField]
    private AudioClip wavingAudioClip; // Audio clip for waving gesture
    [SerializeField]
    private AudioSource audioSource; // AudioSource to play the sound

    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(inputSource);
        previousPosition = transform.position;

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Ensure the device is active
        if (device.isValid)
        {
            // Check if the A button is pressed to enable gestures
            bool aButtonPressed;
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed) && aButtonPressed)
            {
                canDetectGestures = true; // Enable gesture detection if A button is pressed
            }
            else
            {
                canDetectGestures = false; // Disable gesture detection if A button is not pressed
                // Stop any active fire effect if A button is not pressed
                StopFire();
                isPointing = false;
                isWaving = false;
            }

            if (isDetectionPaused)
            {
                detectionPauseTimer -= Time.deltaTime;
                if (detectionPauseTimer <= 0)
                {
                    isDetectionPaused = false;
                }
            }
            else if (canDetectGestures)
            {
                CheckGestures();
            }
        }
    }

    void CheckGestures()
    {
        // Get the position of the controller
        Vector3 currentPosition;
        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out currentPosition))
        {
            // Calculate movement distance (for waving gesture)
            float movementDistance = Vector3.Distance(previousPosition, currentPosition);

            // Check for waving gesture
            if (movementDistance > wavingThreshold)
            {
                if (!isWaving) // Only detect waving if not already waving
                {
                    Debug.Log("Waving gesture detected!");
                    isWaving = true;
                    // Play the waving sound
                    if (audioSource != null && wavingAudioClip != null)
                    {
                        audioSource.PlayOneShot(wavingAudioClip); // Play the sound once
                    }
                    // If pointing is active, stop it when waving is detected
                    if (isPointing)
                    {
                        StopFire();
                        isPointing = false;
                    }
                }
            }
            else
            {
                isWaving = false; // Reset waving detection when movement is below threshold
            }

            // Update previous position for next frame
            previousPosition = currentPosition;
        }

        // Get the rotation of the controller
        Quaternion controllerRotation;
        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out controllerRotation))
        {
            // Get the forward direction of the controller
            Vector3 controllerForward = controllerRotation * Vector3.forward;

            // Check if the controller is pointing straight forward (within a certain threshold)
            if (Vector3.Dot(controllerForward, Vector3.forward) > pointingThreshold && !isWaving) // Ensure pointing is not detected during waving
            {
                if (!isPointing) // If not already pointing, spawn fire
                {
                    Debug.Log("Pointing gesture detected!");
                    SpawnFire(previousPosition);
                }
                isPointing = true;
            }
            else
            {
                if (isPointing && fireEffectInstance != null) // If pointing has ended, stop fire
                {
                    StopFire();
                }
                isPointing = false;
            }
        }
    }

    void SpawnFire(Vector3 position)
    {
        if (fireEffectInstance == null)
        {
            // Instantiate the fire effect at the controller's position
            fireEffectInstance = Instantiate(fireEffectPrefab, position, Quaternion.identity);

            // Parent the fire effect to the controller to follow its movement
            fireEffectInstance.transform.SetParent(this.transform);

            // Optionally, make the fire effect follow the controller's position/rotation
            fireEffectInstance.transform.localPosition = Vector3.zero;
            fireEffectInstance.transform.localRotation = Quaternion.identity;
        }
    }

    void StopFire()
    {
        if (fireEffectInstance != null)
        {
            // Destroy the fire effect when pointing stops
            Destroy(fireEffectInstance);
        }
    }

    void PauseDetection()
    {
        isDetectionPaused = true;
        detectionPauseTimer = detectionPauseTime; 
    }
}
