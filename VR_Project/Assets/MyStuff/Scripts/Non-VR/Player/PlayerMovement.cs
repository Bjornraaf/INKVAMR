using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float mouseSensitivity = 0.1f; // Lower sensitivity for smooth movement
    public Transform playerCamera;

    private CharacterController controller;
    private Vector3 velocity;
    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;

    void Awake()
    {
        controls = new PlayerControls();

        // Bind movement and look actions
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controls.Player.Jump.triggered && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void HandleMouseLook()
    {
        // Apply sensitivity and frame time to smooth the input
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Rotate player around the Y-axis (horizontal rotation)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera around the X-axis (vertical rotation)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp vertical rotation
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
