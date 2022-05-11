using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SVRInput : MonoBehaviour
{
    [HideInInspector]
    public SteamVR_ActionSet actions = SteamVR_Input.GetActionSet("default");

}
