using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    static MusicPlayer musicPlayer;

    public AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip PromptUpdatedSound = null;
    [SerializeField] private AudioClip InputCorrectSound = null;
    [SerializeField] private AudioClip InputIncorrectSound = null;


    private void OnEnable()
    {
        if (GameManager.Instance)
        {
            EventManager.OnPromptUpdated += HandlePromptUpdated;
            EventManager.OnPlayerInputCorrect += PlayCorrectSound;
            EventManager.OnPlayerInputWrong += PlayIncorrectSound;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            EventManager.OnPromptUpdated -= HandlePromptUpdated;
            EventManager.OnPlayerInputCorrect -= PlayCorrectSound;
            EventManager.OnPlayerInputWrong -= PlayIncorrectSound;
        }
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

    // Update is called once per frame
    void Update()
    {
        
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

    public void PlayInputSound(AudioClip in_audioClip, AudioSource in_audioSource = null)
    {
        // if no audioSource is assigned, use the one attached to the AudioManager
        if (!in_audioSource)
            in_audioSource = audioSource;

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

    public void PlayIncorrectSound(GameObject playerGO)
    {
        AudioSource playerAudioSource = playerGO.GetComponent<AudioSource>();

        PlayInputSound(InputIncorrectSound, playerAudioSource);
    }
}
