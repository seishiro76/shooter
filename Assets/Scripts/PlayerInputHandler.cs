using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }

    public float MouseX { get; private set; }
    public float MouseY { get; private set; }

    public bool JumpPressed { get; private set; }

    private void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        JumpPressed = Input.GetKeyDown(KeyCode.Space);
    }
}