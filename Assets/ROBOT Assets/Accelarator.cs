using UnityEngine;

public class WheelRotatorVR : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.right;
    public float rotationSpeed = 100f;

    [Header("Desktop Controls")]
    public KeyCode rotateKey = KeyCode.Space;

    void Update()
    {
        // DESKTOP: Space bar to rotate
        if (Input.GetKey(rotateKey))
        {
            transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
