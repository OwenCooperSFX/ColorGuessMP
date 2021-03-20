using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioSource;

    public List<AudioClip> audioClips;

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
    }

    public void PlayInputSound(AudioClip in_audioClip, AudioSource in_audioSource = null)
    {
        if (!in_audioSource)
            in_audioSource = audioSource;

        in_audioSource.PlayOneShot(in_audioClip);
    }
}
