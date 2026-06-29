using System.Collections;
using UnityEngine;

public class CollectorManager : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] pathPoints;
    public float moveSpeed = 3f;

    [Header("Ball Detection Settings")]
    public Collider intakeTrigger;

    [Header("Controls")]
    public KeyCode intakeKey = KeyCode.E;

    private bool intakeActive = false;
    private Coroutine activeRoutine;

    private void OnEnable()
    {
        if (intakeTrigger != null)
            intakeTrigger.isTrigger = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(intakeKey))
        {
            intakeActive = !intakeActive;
            Debug.Log("Intake " + (intakeActive ? "ON" : "OFF"));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!intakeActive) return;
        if (!other.CompareTag("WildFire")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        // Prevent multiple coroutines fighting each other
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        activeRoutine = StartCoroutine(MoveBallAlongPath(other.gameObject, rb));
    }

    private IEnumerator MoveBallAlongPath(GameObject ball, Rigidbody rb)
    {
        // Save physics state
        Vector3 savedVelocity = rb.velocity;
        Vector3 savedAngularVelocity = rb.angularVelocity;

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Move through path points
        foreach (Transform point in pathPoints)
        {
            if (point == null) continue;

            while (Vector3.Distance(ball.transform.position, point.position) > 0.05f)
            {
                ball.transform.position = Vector3.MoveTowards(
                    ball.transform.position,
                    point.position,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }
        }

        // Restore physics cleanly
        rb.isKinematic = false;
        rb.velocity = savedVelocity;
        rb.angularVelocity = savedAngularVelocity;

        activeRoutine = null;
    }
}