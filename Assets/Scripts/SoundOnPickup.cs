using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(AudioSource))]
public class SoundOnPickup : MonoBehaviour
{
    [Header("References")]
    public Interactable interactable;
    public AudioClip pickupSound;
    public AudioClip releaseSound;

    [Header("Settings")]
    public float pitchModulation = 0;
    public float rechargeTime = 0;

    private AudioSource _audio;
    private float lastPlay = 0;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (pickupSound != null)
            interactable.onAttachedToHand += Event_Attached;
        if (releaseSound != null)
            interactable.onDetachedFromHand += Event_Detached;
    }

    private void Event_Attached(Hand _) =>
        PlaySound(pickupSound);
    private void Event_Detached(Hand _) =>
        PlaySound(releaseSound);

    private void PlaySound(AudioClip clip)
    {
        if (Time.time - lastPlay < rechargeTime)
            return;

        lastPlay = Time.time;
            
        _audio.pitch = 1 + Random.Range(-pitchModulation, pitchModulation);
        _audio.PlayOneShot(clip);
    }
}
