using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/* If SteamVR intialization fails and the fallback camera is activated, we need to update UI canvases to expect
 * interactions from the new active camera.
 * 
 * Ensure script is added to *active* gameobject when Start() is called.
 * 
 * TODO: Look at better ways to implement this? Should be some neater process for post-initialization setup that SteamVR provides.
 */
public class AutoSetEventCamera : MonoBehaviour
{
    public Camera VRCamera;
    public Camera fallbackCamera;

    public Canvas canvas;

    private void Start()
    {
        SteamVR_Action_Boolean headsetOnHead = Player.instance.headsetOnHead;

        // Update when player puts on/removes headset
        headsetOnHead.onChange += (_,__,state) => SetCamera(state);

        // Update immediately on start
        SetCamera(headsetOnHead.active);
    }

    private void SetCamera(bool useMain) =>
        canvas.worldCamera = useMain ? VRCamera : fallbackCamera;

    
}
