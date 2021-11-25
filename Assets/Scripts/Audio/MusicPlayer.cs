using System.Collections;
using UnityEngine;

public enum MusicTrack { Gameplay, MainMenu, Credits }

public class MusicPlayer : MonoBehaviour
{
    public MusicTrack trackPlaying;

    [SerializeField]
    [Range(1f, 2f)]
    float _maxPitch = 1.6f;
    public float maxPitch { get { return _maxPitch; } set { _maxPitch = maxPitch; } }

    [SerializeField]
    private float _pitch = 1f;
    public float pitch { get { return _pitch; } set { _pitch = pitch; } }

    [SerializeField]
    [Range(0f,2f)]
    private float _pitchInterpSpeed = 0.5f;
    public float pitchInterpSpeed { get { return _pitchInterpSpeed; } set { _pitchInterpSpeed = pitchInterpSpeed; } }

    [SerializeField]
    AudioClip gameplayMusic, mainMenuMusic, creditsMusic;

    AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1f;
    }

    void OnEnable()
    {
        EventManager.OnBadThingMoved += SetPitchByPosition;
        EventManager.OnExceededBoundary += ResetPitch;
    }

    void OnDisable()
    {
        EventManager.OnBadThingMoved -= SetPitchByPosition;
        EventManager.OnExceededBoundary -= ResetPitch;
    }

    public void SetMusicTrack(MusicTrack trackToPlay)
    {
        switch (trackToPlay)
        {
            case MusicTrack.Gameplay:
                PlayAudioClip(gameplayMusic);
                break;
            case MusicTrack.MainMenu:
                PlayAudioClip(mainMenuMusic);
                break;
            case MusicTrack.Credits:
                PlayAudioClip(creditsMusic);
                break;
        }
    }

    void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    IEnumerator SetPitchByPositionCoroutine()
    {
        if (TugOfWar.Instance)
        {
            float limit = TugOfWar.Instance.horizontalLimit;
            float xPos = TugOfWar.Instance.badThing_xPos;
            float percentage = Mathf.Abs(xPos / limit);
            float scaledPitch = _maxPitch * percentage;

            //print(scaledPitch);

            if (scaledPitch < 1f)
            {
                scaledPitch = 1f;
            }

            if (percentage >= .5f)
            {
                while (_pitch != scaledPitch)
                {
                    _pitch = Mathf.MoveTowards(_pitch, scaledPitch, _pitchInterpSpeed * Time.deltaTime);

                    audioSource.pitch = _pitch;

                    yield return null;
                }
            }
            yield break;
        }
    }

    IEnumerator ResetPitchCoroutine()
    {
        while (audioSource.pitch != 1f)
        {
            audioSource.pitch = Mathf.MoveTowards(audioSource.pitch, 1f, 3 * _pitchInterpSpeed * Time.deltaTime);
            _pitch = 1f;
            yield return null;
        }

        yield break;
    }

    void ResetPitch()
    {
        //print("Reset Pitch");
        StartCoroutine(ResetPitchCoroutine());
    }

    void SetPitchByPosition()
    {
        //print("Set Pitch");
        StartCoroutine(SetPitchByPositionCoroutine());
    }
}
