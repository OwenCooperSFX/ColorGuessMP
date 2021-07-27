using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip PromptUpdatedSound = null;
    [SerializeField] private AudioClip InputCorrectSound = null;
    [SerializeField] private AudioClip InputIncorrectSound = null;


    private void OnEnable()
    {
        GameManager.Instance.OnPromptUpdated += HandlePromptUpdated;
        GameManager.Instance.OnPlayerInputCorrect += PlayCorrectSound;
        GameManager.Instance.OnPlayerInputIncorrect += PlayIncorrectSound;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPromptUpdated -= HandlePromptUpdated;
        GameManager.Instance.OnPlayerInputCorrect -= PlayCorrectSound;
        GameManager.Instance.OnPlayerInputIncorrect -= PlayIncorrectSound;
    }

    private void Awake()
    {
        Init();

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        // Singleton logic
        Instance = FindObjectOfType<AudioManager>();

        if (Instance && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
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
