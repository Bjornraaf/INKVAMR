using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPicker : MonoBehaviour
{
    public Transform playerCamera;
    public float pickUpRange = 5f;
    public Transform holdPosition;
    private GameObject heldObject;
    public float throwForce = 10f;

    private PlayerControls controls; // Reference to the Input Actions

    void Awake()
    {
        controls = new PlayerControls();

        // Bind pickup and throw actions
        controls.Player.PickUp.performed += ctx => HandlePickUp();
        controls.Player.Throw.performed += ctx => HandleThrow();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void HandlePickUp()
    {
        if (heldObject == null)
            PickUpObject();
        else
            DropObject();
    }

    void HandleThrow()
    {
        if (heldObject != null)
            ThrowObject();
    }

    void PickUpObject()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Pick-Up"))
            {
                heldObject = hit.collider.gameObject;
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = true; // Disable physics to hold the object
                    heldObject.transform.SetParent(holdPosition);
                    
                    // Snap object to the hold position
                    heldObject.transform.localPosition = Vector3.zero;
                    heldObject.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Enable physics again
                heldObject.transform.SetParent(null);
            }

            heldObject = null;
        }
    }

    void ThrowObject()
    {
        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Enable physics again
                rb.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse);
                heldObject.transform.SetParent(null);
            }

            heldObject = null;
        }
    }
}
