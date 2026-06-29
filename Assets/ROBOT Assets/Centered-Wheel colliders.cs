using UnityEngine;
using System.Collections.Generic;

public class OmniRobotKinematicControl : MonoBehaviour
{
    [Header("Wheel Assignments (Assign ALL 6 Individual WheelColliders)")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider midLeftWheel;
    public WheelCollider midRightWheel;
    public WheelCollider backLeftWheel;
    public WheelCollider backRightWheel;

    [Header("Visual Wheel Setup (Optional)")]
    public List<WheelCollider> allWheelCollidersForVisuals = new List<WheelCollider>();
    public List<Transform> allVisualWheels = new List<Transform>();

    [Header("Movement Powers")]
    public float forwardBackwardPower = 2000f;
    public float turnPower = 1500f;
    public float strafePower = 2500f;
    public float brakeTorque = 500f;

    void FixedUpdate()
    {
        // DESKTOP CONTROLS:
        // W/UpArrow = Forward
        // S/DownArrow = Backward
        // Left/Right Arrow or A/D = Turn left/right

        float verticalArrowInput = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) verticalArrowInput = -1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) verticalArrowInput = 1f;

        float horizontalArrowInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) horizontalArrowInput = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) horizontalArrowInput = -1f;

        // Gamepad support
        float verticalGamepad = Input.GetAxis("Vertical");
        float horizontalGamepad = Input.GetAxis("Horizontal");

        if (Mathf.Abs(verticalGamepad) > 0.1f) verticalArrowInput = verticalGamepad;
        if (Mathf.Abs(horizontalGamepad) > 0.1f) horizontalArrowInput = horizontalGamepad;

        // Get all active wheels
        List<WheelCollider> activeWheels = new List<WheelCollider>
        {
            frontLeftWheel, frontRightWheel,
            midLeftWheel, midRightWheel,
            backLeftWheel, backRightWheel
        };
        activeWheels.RemoveAll(item => item == null);

        // Reset wheels
        foreach (WheelCollider wheel in activeWheels)
        {
            wheel.motorTorque = 0f;
            wheel.brakeTorque = 0f;
            wheel.steerAngle = 0f;
        }

        if (verticalArrowInput != 0f)
        {
            // Forward/backward
            foreach (WheelCollider wheel in activeWheels)
            {
                wheel.motorTorque = verticalArrowInput * forwardBackwardPower;
            }
        }
        else if (horizontalArrowInput != 0f)
        {
            // Turning
            foreach (WheelCollider wheel in activeWheels)
            {
                if (wheel.transform.localPosition.x < 0) // Left side wheels
                    wheel.motorTorque = horizontalArrowInput * turnPower;
                else // Right side wheels
                    wheel.motorTorque = -horizontalArrowInput * turnPower;
            }
        }
        else
        {
            // No input — brake
            foreach (WheelCollider wheel in activeWheels)
            {
                wheel.brakeTorque = brakeTorque;
            }
        }

        UpdateWheelVisuals();
    }

    void UpdateWheelVisuals()
    {
        if (allWheelCollidersForVisuals.Count != allVisualWheels.Count) return;

        for (int i = 0; i < allWheelCollidersForVisuals.Count; i++)
        {
            WheelCollider wheelCol = allWheelCollidersForVisuals[i];
            Transform wheelTransform = allVisualWheels[i];
            if (wheelCol == null || wheelTransform == null) continue;

            wheelCol.GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheelTransform.position = pos;
            wheelTransform.rotation = rot;
        }
    }
}
