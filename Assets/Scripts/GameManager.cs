using UnityEngine;
using System;

/*
NOTES:
    TODO:
    - automate top prompt random color              
    - give players color control layout             DONE
    - reward score for matching color               IN PROGRESS       
    - refresh random color for prompt               
    - re-randomize the winning player's controls    

    TODO:
    - Animations for player input
    - Animations for new random color prompt
    - Animations for reward/penalty

    TODO:
    - Winner screen
    - Tutorial

    TODO:
    - Sound design, music
*/

public enum ColorOptions { invalid, red, blue, yellow, green, random }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ColorOptions colorOptions;

    private AudioSource p1AudioSource;
    private AudioSource p2AudioSource;

    // Material refs
    private Material colorPromptMat;
    //private Material p1ColorMat;
    //private Material p2ColorMat;

    [Header("GameObjects")]
    // GameObject refs
    public GameObject colorPromptGO;
    public GameObject p1ColorInputGO;
    public GameObject p2ColorInputGO;

    [Header("Gameplay")]
    // Score
    public int p1Score;
    public int p2Score;

    // Events
    public delegate void PlayerInputCorrect(ColorObject player);
    public event PlayerInputCorrect OnPlayerInputCorrect;

    private void Awake()
    {
        Init();
        colorPromptMat = colorPromptGO.GetComponent<MeshRenderer>().material;
        //p1ColorMat = p1ColorInputGO.GetComponent <MeshRenderer>().material;
        //p2ColorMat = p2ColorInputGO.GetComponent<MeshRenderer>().material;

        // Create P1 and P2 Audio Sources.
        p1AudioSource = p1ColorInputGO.AddComponent<AudioSource>();
        p1AudioSource.panStereo = -0.6f;

        p2AudioSource = p2ColorInputGO.AddComponent<AudioSource>();
        p2AudioSource.panStereo = 0.6f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.DisplayRandomColor() != ColorOptions.invalid)
        {
            UpdateColor(colorPromptGO);
            AudioManager.Instance.PlayInputSound(AudioManager.Instance.audioClips[0]);
        }


        if (PlayerController.Instance.GetPlayer1Input() != ColorOptions.invalid)
        {
            if (CompareInputToPrompt(PlayerController.Instance.GetPlayer1Input()))
            {
                AudioManager.Instance.PlayInputSound(AudioManager.Instance.audioClips[2], p1AudioSource);
                print("P1 input correct!");
            }
            else
            {
                AudioManager.Instance.PlayInputSound(AudioManager.Instance.audioClips[1], p1AudioSource);
            }

        }


        if (PlayerController.Instance.GetPlayer2Input() != ColorOptions.invalid)
        {
            if (CompareInputToPrompt(PlayerController.Instance.GetPlayer2Input()))
            {
                AudioManager.Instance.PlayInputSound(AudioManager.Instance.audioClips[2], p2AudioSource);
                print("P2 input correct!");
            }
            else
            {
                AudioManager.Instance.PlayInputSound(AudioManager.Instance.audioClips[1], p2AudioSource);
            }

        }

    }

    public void UpdateColor(GameObject colorGO)
    {
        ColorOptions inputColor = 0;
        ColorOptions newColorOption = 0;
        Material targetMaterial = colorGO.GetComponent<MeshRenderer>().material;

        // Check which type of input we received NOTE: this is now redundant. Only used for prompt.
        if (colorGO == colorPromptGO) // update prompt material
        {
            // Should probably make this into its own method.
            inputColor = PlayerController.Instance.DisplayRandomColor();
            ColorObject colorObject = colorGO.GetComponent<ColorObject>();
            newColorOption = RandomizeColor();
            colorObject.currentColor = newColorOption;
            colorObject.LightColorPrompt();
        }

        Color newColor = Color.black;  

        switch (newColorOption)
        {
            case ColorOptions.blue:
                newColor = Color.blue;
                break;
            case ColorOptions.green:
                newColor = Color.green;
                break;
            case ColorOptions.red:
                newColor = Color.red;
                break;
            case ColorOptions.yellow:
                newColor = Color.yellow;
                break;
            case ColorOptions.invalid:
                Debug.LogWarning("Invalid color!");
                break;
        }

        if (targetMaterial.color != newColor)
            targetMaterial.color = newColor;
    }

    public ColorOptions RandomizeColor()
    {
        int rndInt = UnityEngine.Random.Range(1, 5);

        ColorOptions rndColor = (ColorOptions)rndInt;

        return rndColor;
    }

    void Init()
    {
        // Singleton logic
        Instance = FindObjectOfType<GameManager>();
        
        if (Instance && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    bool  CompareInputToPrompt(ColorOptions input)
    {
        ColorOptions promptColor = colorPromptGO.GetComponent<ColorObject>().currentColor;

        if (input == promptColor)
        {
            return true;
        }

        return false;
    }
}
