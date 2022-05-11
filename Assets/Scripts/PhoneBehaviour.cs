
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PhoneBehaviour : MonoBehaviour
{

    public TextMeshPro phoneScreen;
    public Transform responseCanvas;
    public ReturnItem itemReturn;
    public AudioClip ringing;
    public AudioClip busy;
    public AudioClip dialTone;

    [HideInInspector]
    public Caller caller;

    private AudioSource _audio;

    private PhoneState _state;
    private Coroutine _currentClip;

    public void StartCall(Caller caller)
    {
        this.caller = caller;
        SetState(PhoneState.Ringing);
    }

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        itemReturn.OnPickup += Event_Pickup;
        itemReturn.OnPutdown += Event_Putdown;
    }

    

    public void SetState(PhoneState state)
    {
        switch (state)
        {
            case PhoneState.Idle:
                phoneScreen.SetText("----------");
                _audio.Stop();
                responseCanvas.gameObject.SetActive(false);
                if (itemReturn.IsHeld())
                {
                    _audio.clip = dialTone;
                    _audio.Play();
                }
                    break;
            case PhoneState.Ringing:
                phoneScreen.SetText(caller.number);
                if (itemReturn.IsHeld())
                {
                    responseCanvas.gameObject.SetActive(true);
                    PlayDeferred(1, caller.data.mainCall);
                    SetState(PhoneState.InCall);
                    break;
                }
                _audio.clip = ringing;
                _audio.Play();
                break;
        }

        _state = state;
    }

    private void Event_Putdown()
    {
        switch (_state)
        {
            case PhoneState.Idle:
                _audio.Stop();
                break;
            case PhoneState.InCall:
                _audio.minDistance = 1;
                break;
            case PhoneState.Ringing:
                break;
        }

    }

    private void Event_Pickup()
    {
        _audio.minDistance = 0.2f;

        switch(_state)
        {
            case PhoneState.Idle:
                _audio.clip = dialTone;
                _audio.Play();
                break;
            case PhoneState.Ringing:
                responseCanvas.gameObject.SetActive(true);
                PlayDeferred(1, caller.data.mainCall);
                SetState(PhoneState.InCall);
                break;
        }
    }

    public void Repeat()
    {
        if (_state == PhoneState.InCall)
        {
            AudioClip randomClip = caller.RandomRepeatClip();

            PlayDeferred(0.3f, randomClip);
        }
    }

    public void Confirm()
    {
        PlayDeferred(0.3f, caller.data.confirmLine);

        StartCoroutine(Utility.Defer(caller.data.confirmLine.length + 0.3f, () =>
        {
            SetState(PhoneState.Idle);
            LevelManager.instance.SubmitGuess(false);
        }));
    }

    public void Refuse()
    {
        SetState(PhoneState.Idle);
        LevelManager.instance.SubmitGuess(true);
    }

    public void PlayDeferred(float delay, AudioClip clip = null)
    {
        if (_currentClip != null)
            StopCoroutine(_currentClip);

        _currentClip = StartCoroutine(DeferredClipStart(delay, clip));
    }
    public IEnumerator DeferredClipStart(float delay, AudioClip clip = null)
    {
        _audio.Stop();
        yield return new WaitForSeconds(delay);

        if (clip == null)
            _audio.Play();
        else
            _audio.PlayOneShot(clip);
    }

    public enum PhoneState
    { 
        Idle,
        InCall,
        Ringing
    }

}
