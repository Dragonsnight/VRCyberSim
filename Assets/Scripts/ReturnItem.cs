using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Collider))]
public class ReturnItem : MonoBehaviour
{
    [Tooltip("The item that will be returned back to this point")]
    public GameObject item;

    public AudioSource audioSource;
    [Tooltip("Sound that plays when the item reaches it's target")]
    public AudioClip returnSound;
    public AudioClip pickupSound;

    public ReturnType returnType;

    public float delay;


    public float returnTime = 1;

    [HideInInspector]
    public event Default OnPickup;
    public event Default OnPutdown;

    private Rigidbody _rb;
    private Interactable _interactable;

    private Coroutine _returnCoroutine;
    private bool _inRange = true;
    private bool _hasMoved = false;
    private float _time = 0;

    void Awake()
    {
        _rb = item.GetComponent<Rigidbody>();
        _interactable = item.GetComponent<Interactable>();
    }
    void Start()
    { 
        _interactable.onAttachedToHand += Event_Grabbed;
        _interactable.onDetachedFromHand += Event_UnGrabbed;
        _rb.isKinematic = true;
    }

    public void Event_Grabbed(Hand hand)
    {
        if (!_hasMoved && pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound);
            OnPickup?.Invoke();
        }
        _time = 0;

        _hasMoved = true;
    }

    public void Event_UnGrabbed(Hand hand)
    {
        _rb.isKinematic = false;
    }

    public bool IsHeld()
    {
        return _interactable.attachedToHand != null;
    }

    void Update()
    {
        // Skip if already returning
        if (_returnCoroutine != null)
            return;

        // Skip if the item is already in place
        if (!_hasMoved)
            return;

        // Skip if being held
        if (IsHeld())
            return;

        switch (returnType)
        {
            case ReturnType.OutRange:
                if (_inRange)
                    break;

                if (_time > delay)
                    _returnCoroutine = StartCoroutine(Return());

                _time += Time.deltaTime;

                break;

            case ReturnType.InRange:
                if (_inRange)
                    _returnCoroutine = StartCoroutine(Return());
                break;
            case ReturnType.Always:
                _returnCoroutine = StartCoroutine(Return());
                break;
        }
    }

    private void OnTriggerEnter(Collider _) =>
        _inRange = true;

    private void OnTriggerExit(Collider _) =>
        _inRange = false;

    private IEnumerator Return()
    {
        Transform itemTrf = item.transform;

        Vector3 currentPos = itemTrf.position;
        Quaternion currentRot = itemTrf.rotation;

        foreach (Collider col in item.GetComponentsInChildren<Collider>())
            col.enabled = false;


        float t = 0;
        while (t < returnTime)
        {
            t += Time.deltaTime;

            itemTrf.SetPositionAndRotation(
                Vector3.Lerp(currentPos, transform.position, t / returnTime), 
                Quaternion.Lerp(currentRot, transform.rotation, t / returnTime));

            yield return null;
        }

        if (returnSound != null)
            audioSource.PlayOneShot(returnSound);

        itemTrf.SetPositionAndRotation(transform.position, transform.rotation);

        foreach (Collider col in item.GetComponentsInChildren<Collider>())
            col.enabled = true;


        _time = 0;
        _hasMoved = false;
        _rb.isKinematic = true;
        OnPutdown?.Invoke();
        _returnCoroutine = null;
    }


    public enum ReturnType
    { 
        OutRange,
        InRange,
        Always
    }

    public delegate void Default();

}
