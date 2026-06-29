using UnityEngine;
using UnityEngine.InputSystem;

public class VRDroneController : MonoBehaviour
{
    [Header("Flight Settings")]
    public float moveSpeed = 10f;       // Speed for forward/backward/left/right
    public float elevationSpeed = 5f;   // Speed for going straight up/down
    public float rotationSpeed = 60f;   // Turning speed in degrees per second

    [Header("VR Inputs")]
    [Tooltip("Map this to your Left Hand Thumbstick (Vector2)")]
    public InputActionReference leftThumbstick;

    [Tooltip("Map this to your Right Hand Thumbstick (Vector2)")]
    public InputActionReference rightThumbstick;

    private void Update()
    {
        HandleLeftController();
        HandleRightController();
    }

    private void HandleLeftController()
    {
        // Safety check to ensure the input is assigned
        if (leftThumbstick != null && leftThumbstick.action != null)
        {
            // Read Left Thumbstick values: X = Left/Right, Y = Forward/Backward
            Vector2 leftInput = leftThumbstick.action.ReadValue<Vector2>();

            // Calculate movement relative to where the drone is currently facing
            Vector3 movement = (transform.forward * leftInput.y) + (transform.right * leftInput.x);

            // Apply the movement to the drone's position
            transform.position += movement * moveSpeed * Time.deltaTime;
        }
    }

    private void HandleRightController()
    {
        // Safety check to ensure the input is assigned
        if (rightThumbstick != null && rightThumbstick.action != null)
        {
            // Read Right Thumbstick values: X = Rotate Left/Right, Y = Up/Down
            Vector2 rightInput = rightThumbstick.action.ReadValue<Vector2>();

            // 1. Handle Elevation (Up/Down movement)
            // We use Vector3.up (world space up) so the drone doesn't fly diagonally if it's tilted
            transform.position += Vector3.up * rightInput.y * elevationSpeed * Time.deltaTime;

            // 2. Handle Rotation (Yaw)
            // Rotate around the Y axis based on the horizontal thumbstick push
            transform.Rotate(Vector3.up, rightInput.x * rotationSpeed * Time.deltaTime);
        }
    }
}