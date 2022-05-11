using UnityEngine;
using System.Reflection;
using System;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "CallData", menuName = "Data/Caller")]
public class CallData : ScriptableObject
{
    public string callerName;

    [Tooltip("The main call audio that will play when the phone is picked up")]
    public AudioClip mainCall;

    [Space]

    [Tooltip("If player asks the caller to repeat information, one of these clips will play")]
    public AudioClip[] repeatLines;

    public AudioClip confirmLine;




}
