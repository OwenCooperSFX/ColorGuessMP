using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicTrack { Gameplay, MainMenu, Credits }

public class MusicPlayer : MonoBehaviour
{
    public MusicTrack trackPlaying;

    private float _maxPitch = 1.6f;
    public float maxPitch { get { return _maxPitch; } set { _maxPitch = maxPitch; } }

    private float _pitch = 1f;
    public float pitch { get { return _pitch; } set { _pitch = pitch; } }

    [SerializeField] AudioClip gameplayMusic, mainMenuMusic, creditsMusic;

    AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1f;
    }

    void OnEnable()
    {
       //TugOfWar.Instance.OnBadThingMoved += SetPitchByPosition;
        TugOfWar.Instance.OnExceededBoundary += ResetPitch;
    }

    void OnDisable()
    {
        //TugOfWar.Instance.OnBadThingMoved -= SetPitchByPosition;
        TugOfWar.Instance.OnExceededBoundary -= ResetPitch;
    }

    private void Update()
    {
        SetPitchByPosition();
    }

    public void SetMusicTrack(MusicTrack trackToPlay)
    {
        switch (trackToPlay)
        {
            case MusicTrack.Gameplay:
                PlayAudioClip(gameplayMusic);
                break;
            case MusicTrack.MainMenu:
                PlayAudioClip(null);
                break;
            case MusicTrack.Credits:
                PlayAudioClip(null);
                break;
        }
    }

    void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void SetPitchByPosition()
    {
        float limit = TugOfWar.Instance.horizontalLimit;
        float xPos = TugOfWar.Instance.badThing.transform.localPosition.x;
        float percentage = Mathf.Abs(xPos / limit);
        float scaledPitch = _maxPitch * percentage;

        //print(scaledPitch);

        if (scaledPitch > 1f)
        {
            // StartCoroutine(LerpToPitch(scaledPitch, .01f));
            audioSource.pitch = scaledPitch;
        }
    }

    IEnumerator ResetPitchCoroutine()
    {
        while (audioSource.pitch != 1f)
        {
            audioSource.pitch = Mathf.MoveTowards(audioSource.pitch, 1f, 1f);
            yield return null;
        }
    }

    IEnumerator LerpToPitch(float targetPitch, float maxDelta)
    {
        while (audioSource.pitch != targetPitch)
        {
            audioSource.pitch = Mathf.MoveTowards(audioSource.pitch, targetPitch, maxDelta);
            yield return null;
        }
    }

    void ResetPitch()
    {
        print("ResetPitch");
        StartCoroutine(ResetPitchCoroutine());
    }
}
