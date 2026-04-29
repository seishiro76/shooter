using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Mouse Look")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Jump and Gravity")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -20f;

    private CharacterController characterController;
    private Transform cachedTransform;

    private float verticalVelocity;
    private float cameraVerticalRotation;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cachedTransform = transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        MoveAndJump();
    }

    private void MoveAndJump()
    {
        bool isGrounded = characterController.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = cachedTransform.right * horizontalInput +
                                cachedTransform.forward * verticalInput;

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cachedTransform.Rotate(Vector3.up * mouseX);

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }
}