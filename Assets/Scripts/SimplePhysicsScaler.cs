using UnityEngine;

public class SimplePhysicsScaler : MonoBehaviour
{
    [Header("Before Scaling - Record These Values")]
    [SerializeField] public float originalRigidbodyMass = 100f;
    [SerializeField] public float originalWheelMass = 20f;
    [SerializeField] public float originalWheelSpringForce = 35000f;
    [SerializeField] public float originalWheelSpringDamper = 4500f;

    [Header("Scaling")]
    [SerializeField] public Rigidbody mainRigidbody;
    [SerializeField] public WheelCollider[] allWheels;
    [SerializeField] public float oldScale = 10.27f;
    [SerializeField] public float newScale = 1f;

    [ContextMenu("STEP 1: Save Current Physics Values")]
    public void SaveCurrentValues()
    {
        if (mainRigidbody == null)
        {
            Debug.LogError("Assign mainRigidbody!");
            return;
        }

        originalRigidbodyMass = mainRigidbody.mass;
        
        if (allWheels.Length > 0 && allWheels[0] != null)
        {
            originalWheelMass = allWheels[0].mass;
            JointSpring spring = allWheels[0].suspensionSpring;
            originalWheelSpringForce = spring.spring;
            originalWheelSpringDamper = spring.damper;
        }

        Debug.Log("✓ Saved: RB Mass=" + originalRigidbodyMass + ", Wheel Mass=" + originalWheelMass);
    }

    [ContextMenu("STEP 2: Apply Scaling")]
    public void ApplyScaling()
    {
        if (mainRigidbody == null || allWheels.Length == 0)
        {
            Debug.LogError("Assign Rigidbody and Wheels!");
            return;
        }

        // Calculate scaling factors
        float volumeScale = (newScale / oldScale) * (newScale / oldScale) * (newScale / oldScale);
        float linearScale = newScale / oldScale;

        // Scale rigidbody mass
        float newMass = originalRigidbodyMass * volumeScale;
        mainRigidbody.mass = newMass;
        Debug.Log("✓ Rigidbody: " + originalRigidbodyMass + " → " + newMass);

        // Scale each wheel
        foreach (WheelCollider wheel in allWheels)
        {
            if (wheel == null) continue;

            // Scale wheel mass
            wheel.mass = originalWheelMass * volumeScale;

            // Scale suspension spring
            JointSpring spring = wheel.suspensionSpring;
            spring.spring = originalWheelSpringForce * linearScale;
            spring.damper = originalWheelSpringDamper * linearScale;
            wheel.suspensionSpring = spring;

            Debug.Log("✓ Wheel '" + wheel.name + "' scaled. Mass: " + wheel.mass);
        }

        Debug.Log("✓ SCALING COMPLETE! The robot should now work at scale " + newScale);
    }
}
