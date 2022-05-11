using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Modified solution
// https://forum.unity.com/threads/steam-vr-plugin-reset-position-and-orientation.389588/

public class PositionManager : MonoBehaviour
{
    [Tooltip("Desired position & orientation for the player")]
    public Transform desiredPosition;

    public Transform playerHead;
    public Transform cameraRig;

    private void Start()
    {
        SteamVR_Actions.default_ResetPos.onStateDown += Event_ResetPos;
    }

    private void Event_ResetPos(SteamVR_Action_Boolean _, SteamVR_Input_Sources __)
    {
        ResetCamera();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ResetCamera();
        
    }

    public void ResetCamera()
    {
        if (desiredPosition == null)
        {
            Debug.LogError("No desired position to return the player to has been set", transform);
            return;
        }

        if (playerHead == null || cameraRig == null)
        {
            Debug.LogError("Unassigned steamVR object(s)", transform);
            return;
        }

        // Rotation
        float offsetAngle = playerHead.rotation.eulerAngles.y;

        cameraRig.Rotate(0f, -offsetAngle, 0f);

        // Position
        Vector3 offsetPos = playerHead.position - cameraRig.position;
        Vector3 newPos = (desiredPosition.position - offsetPos);
        newPos.y = cameraRig.position.y;
        cameraRig.position = newPos;

    }
}
