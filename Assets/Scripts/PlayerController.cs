using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;

    private CharacterController characterController;
    private Transform cachedTransform;

    private float verticalVelocity;
    private float cameraVerticalRotation;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cachedTransform = transform;

        if (inputHandler == null)
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        MoveAndJump();
        Look();
    }

    private void MoveAndJump()
    {
        bool isGrounded = characterController.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        Vector3 moveDirection =
            cachedTransform.right * inputHandler.HorizontalInput +
            cachedTransform.forward * inputHandler.VerticalInput;

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        if (isGrounded && inputHandler.JumpPressed)
        {
            verticalVelocity = jumpForce;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMovement = moveDirection * moveSpeed;
        finalMovement.y = verticalVelocity;

        characterController.Move(finalMovement * Time.deltaTime);
    }

    private void Look()
    {
        float mouseX = inputHandler.MouseX * mouseSensitivity;
        float mouseY = inputHandler.MouseY * mouseSensitivity;

        cachedTransform.Rotate(Vector3.up * mouseX);

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }
}