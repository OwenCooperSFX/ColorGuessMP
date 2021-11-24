using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    static MusicPlayer musicPlayer;

    private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip PromptUpdatedSound;
    [SerializeField] AudioClip InputCorrectSound;
    [SerializeField] AudioClip InputWrongSound;


    private void OnEnable()
    {
        EventManager.OnPromptUpdated += HandlePromptUpdated;
        EventManager.OnPlayerInputCorrect += PlayCorrectSound;
        EventManager.OnPlayerInputWrong += PlayWrongSound;
    }

    private void OnDisable()
    {
        EventManager.OnPromptUpdated -= HandlePromptUpdated;
        EventManager.OnPlayerInputCorrect -= PlayCorrectSound;
        EventManager.OnPlayerInputWrong -= PlayWrongSound;
    }

    private void Awake()
    {
        Init();

        audioSource = GetComponent<AudioSource>();

        musicPlayer = transform.GetChild(0).GetComponent<MusicPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer.SetMusicTrack(MusicTrack.Gameplay);
    }

    void Init()
    {
        if (Instance && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void PlayInputSound(AudioClip in_audioClip = null, AudioSource in_audioSource = null)
    {
        // if no audioSource is specified, use the one attached to the AudioManager
        if (!in_audioSource)
            in_audioSource = audioSource;

        // if no audioClip is specified, use a default
        if (!in_audioClip)
            in_audioClip = PromptUpdatedSound;

        in_audioSource.PlayOneShot(in_audioClip);
    }

    public void HandlePromptUpdated()
    {
        PlayInputSound(PromptUpdatedSound);
    }

    public void PlayCorrectSound(GameObject playerGO)
    {
        AudioSource playerAudioSource = playerGO.GetComponent<AudioSource>();

        PlayInputSound(InputCorrectSound, playerAudioSource);
    }

    public void PlayWrongSound(GameObject playerGO)
    {
        AudioSource playerAudioSource = playerGO.GetComponent<AudioSource>();

        PlayInputSound(InputWrongSound, playerAudioSource);
    }

    public void PlayUISliderMoveSound()
    {
        PlayInputSound();
    }

    public void PlayUISliderLockSound()
    {
        PlayInputSound();
    }
}
