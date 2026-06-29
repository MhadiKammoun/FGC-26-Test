using UnityEngine;

public class SimpleIntake : MonoBehaviour
{
    [Header("Settings")]
    public float intakeForce = 10f;
    public Vector3 intakeDirection = Vector3.forward;

    [Header("Roller Visual")]
    public Transform roller;
    public float rollerSpeed = 360f;

    [Header("Desktop Controls")]
    public KeyCode intakeToggleKey = KeyCode.F;

    private bool intakeActive = false;
    private bool previousKeyState = false;

    private void Update()
    {
        // DESKTOP: F key to toggle intake
        bool keyPressed = Input.GetKey(intakeToggleKey);

        // Detect rising edge (key just pressed)
        if (keyPressed && !previousKeyState)
        {
            intakeActive = !intakeActive;
            Debug.Log("Intake " + (intakeActive ? "ON" : "OFF"));
        }

        previousKeyState = keyPressed;

        // Rotate roller if intake is active
        if (intakeActive && roller != null)
        {
            roller.Rotate(Vector3.forward * rollerSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!intakeActive) return;

        if (other.CompareTag("WildFire"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.AddForce(transform.TransformDirection(intakeDirection) * intakeForce);
            }
        }
    }
}
