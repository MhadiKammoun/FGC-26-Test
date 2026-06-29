using UnityEngine;

public class HangingRotater : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.right;
    public float rotationSpeed = 50f;

    [Header("Desktop Controls")]
    public KeyCode positiveKey = KeyCode.T;   // Clockwise
    public KeyCode negativeKey = KeyCode.G;   // Counter-clockwise

    void Update()
    {
        // DESKTOP: T = Rotate positive, G = Rotate negative
        bool positivePressed = Input.GetKey(positiveKey);
        bool negativePressed = Input.GetKey(negativeKey);

        // Rotate in positive direction
        if (positivePressed)
        {
            transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.Self);
        }

        // Rotate in negative direction
        if (negativePressed)
        {
            transform.Rotate(-rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
