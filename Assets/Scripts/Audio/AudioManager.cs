using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    static MusicPlayer musicPlayer;

    private AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip PromptUpdatedSound;
    [SerializeField] AudioClip InputCorrectSound;
    [SerializeField] AudioClip InputWrongSound;

    [Header("Wwise Startup Soundbanks")]
    [SerializeField] List<AK.Wwise.Bank> startupSoundbanks;

    public List<AK.Wwise.Bank> StartupSoundBanks { get => startupSoundbanks; }

    [Header("Wwise Events")]
    [SerializeField] AK.Wwise.Event promptUpdated;
    [SerializeField] AK.Wwise.Event inputCorrect;
    [SerializeField] AK.Wwise.Event inputWrong;
    [SerializeField] AK.Wwise.Event explosion;

    public AK.Wwise.Event PromptUpdated { get => promptUpdated; }
    public AK.Wwise.Event InputCorrect { get => inputCorrect; }
    public AK.Wwise.Event InputWrong { get => inputWrong; }
    public AK.Wwise.Event Explosion { get => explosion; }


    private void OnEnable()
    {
        EventManager.OnPromptUpdated += HandlePromptUpdated;
        EventManager.OnPlayerInputCorrect += PlayCorrectSound;
        EventManager.OnPlayerInputWrong += PlayWrongSound;
        EventManager.OnExceededBoundary += PlayExplosionSound;
    }

    private void OnDisable()
    {
        EventManager.OnPromptUpdated -= HandlePromptUpdated;
        EventManager.OnPlayerInputCorrect -= PlayCorrectSound;
        EventManager.OnPlayerInputWrong -= PlayWrongSound;
        EventManager.OnExceededBoundary -= PlayExplosionSound;
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

        LoadSoundbanks(startupSoundbanks);
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

    public void PlayInputSound(AK.Wwise.Event wwiseEvent, GameObject source)
    {
        if (wwiseEvent.IsValid() && source)
            wwiseEvent.Post(source);
    }

    public void HandlePromptUpdated()
    {
        //PlayInputSound(PromptUpdatedSound);

        PlayInputSound(promptUpdated, gameObject);
    }

    public void PlayCorrectSound(GameObject playerGO)
    {
        //AudioSource playerAudioSource = playerGO.GetComponent<AudioSource>();
        //PlayInputSound(InputCorrectSound, playerAudioSource);

        PlayInputSound(inputCorrect, playerGO);
    }

    public void PlayWrongSound(GameObject playerGO)
    {
        //AudioSource playerAudioSource = playerGO.GetComponent<AudioSource>();
        //PlayInputSound(InputWrongSound, playerAudioSource);

        PlayInputSound(inputWrong, playerGO);
    }

    private void LoadSoundbanks(List<AK.Wwise.Bank> bankList = null)
    {
        if (bankList.Count > 0)
        {
            foreach (var bank in bankList)
            {
                bank.LoadAsync();
            }
        }
    }

    public void PlayExplosionSound()
    {
        explosion.Post(gameObject);
    }
}
