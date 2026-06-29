using UnityEngine;

public class LimitedRotatorVR : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.right;
    public float rotationSpeed = 90f;

    [Header("Limits")]
    public float minAngle = -45f;
    public float maxAngle = 45f;

    [Header("Desktop Controls")]
    public KeyCode positiveKey = KeyCode.E;   // Rotate positive direction
    public KeyCode negativeKey = KeyCode.Q;   // Rotate negative direction

    private float currentAngle = 0f;

    void Update()
    {
        // DESKTOP: E = Rotate positive, Q = Rotate negative (with limits)
        float rotationInput = 0f;

        if (Input.GetKey(positiveKey))
            rotationInput = 1f;
        else if (Input.GetKey(negativeKey))
            rotationInput = -1f;

        if (rotationInput != 0f)
        {
            float delta = rotationSpeed * rotationInput * Time.deltaTime;
            float newAngle = Mathf.Clamp(currentAngle + delta, minAngle, maxAngle);

            // Apply only allowed delta
            float appliedDelta = newAngle - currentAngle;
            transform.Rotate(rotationAxis * appliedDelta, Space.Self);

            // Update stored angle
            currentAngle = newAngle;
        }
    }
}
