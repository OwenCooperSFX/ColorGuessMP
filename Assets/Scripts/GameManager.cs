using UnityEngine;
using TMPro;

/*
NOTES:
    TODO:
    - Get event system working                      DONE

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

    [Header("GameObjects")]
    // GameObject refs
    public GameObject colorPromptGO;
    public GameObject p1ColorInputGO;
    public GameObject p2ColorInputGO;

    [Header("Gameplay")]
    [SerializeField] private float timeBetweenRounds = 3f;
    [SerializeField] private float nextRoundTimer = 0f;

    [SerializeField] private float roundTimeLimit = 10f;
    [SerializeField] private float roundTimer = 0f;

    private bool isBetweenRounds = false;

    [Header("Scoring: Player 1")]
    [SerializeField] private TextMeshProUGUI p1ScoreText;
    public int p1Score = 0000;
    public int p1Attempts = 0;
    public float p1CorrectAnswerTime = 0f;

    [Header("Scoring: Player 2")]
    [SerializeField] private TextMeshProUGUI p2ScoreText;
    public int p2Score = 0000;
    public int p2Attempts = 0;
    public float p2CorrectAnswerTime = 0f;

    // Events
    public delegate void PromptUpdated();
    public event PromptUpdated OnPromptUpdated;

    public delegate void PlayerInputCorrect(GameObject playerInputGO);
    public event PlayerInputCorrect OnPlayerInputCorrect;

    public delegate void PlayerInputIncorrect(GameObject playerInputGO);
    public event PlayerInputIncorrect OnPlayerInputIncorrect;

    private void OnEnable()
    {
        // TODO: setup event subscribers
        //PlayerController.Instance.OnP1Input += HandleP1Input;
        //PlayerController.Instance.OnP2Input += HandleP2Input;

        OnPlayerInputCorrect += HandlePlayerCorrect;
    }

    private void OnDisable()
    {
        //PlayerController.Instance.OnP1Input -= HandleP1Input;
        //PlayerController.Instance.OnP2Input -= HandleP2Input;

        OnPlayerInputCorrect -= HandlePlayerCorrect;
    }

    private void Awake()
    {
        Init();

        // Create P1 and P2 Audio Sources.
        p1AudioSource = p1ColorInputGO.AddComponent<AudioSource>();
        p1AudioSource.panStereo = -0.6f;

        p2AudioSource = p2ColorInputGO.AddComponent<AudioSource>();
        p2AudioSource.panStereo = 0.6f;

        // Set score texts
        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();

        // Clear timers
        roundTimer = roundTimeLimit;
        nextRoundTimer = timeBetweenRounds;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isBetweenRounds)
        {
            if (nextRoundTimer < timeBetweenRounds)
            {
                nextRoundTimer += Time.deltaTime;
            }
            else
            {
                StartNewRound();
            }
        }

        if (roundTimer < roundTimeLimit)
            roundTimer += Time.deltaTime;

        //temp setup to get things going manually
        if (PlayerController.Instance.InitialSpaceInput() != ColorOptions.invalid)
        {
            StartNewRound();
        }

        // Don't continue if we are between rounds
        if (isBetweenRounds)
            return;

        if (PlayerController.Instance.GetPlayer1Input() != ColorOptions.invalid)
        {
            if (InputEqualsPrompt(PlayerController.Instance.GetPlayer1Input()))
            {
                // Player 1 is correct
                p1Score = UpdatePlayerScore(p1Score, p1ScoreText);
                OnPlayerInputCorrect?.Invoke(p1ColorInputGO);
                Debug.Log("Call Event: " + OnPlayerInputCorrect + " (" + p1ColorInputGO.name + ").");
            }
            else
            {
                // Player 1 is wrong
                OnPlayerInputIncorrect?.Invoke(p1ColorInputGO);
                Debug.Log("Call Event: " + OnPlayerInputIncorrect + " (" + p1ColorInputGO.name + ").");
            }
        }


        if (PlayerController.Instance.GetPlayer2Input() != ColorOptions.invalid)
        {
            if (InputEqualsPrompt(PlayerController.Instance.GetPlayer2Input()))
            {
                // Player 2 is correct
                p2Score = UpdatePlayerScore(p2Score, p2ScoreText);
                OnPlayerInputCorrect?.Invoke(p2ColorInputGO);
                Debug.Log("Call Event: " + OnPlayerInputCorrect + " (" + p2ColorInputGO.name + ").");
            }
            else
            {
                // Player 2 is wrong
                OnPlayerInputIncorrect?.Invoke(p2ColorInputGO);
                Debug.Log("Call Event: " + OnPlayerInputIncorrect + " (" + p2ColorInputGO.name + ").");
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
        DontDestroyOnLoad(gameObject);
    }

    bool  InputEqualsPrompt(ColorOptions input)
    {
        ColorOptions promptColor = colorPromptGO.GetComponent<ColorObject>().currentColor;

        if (input == promptColor)
            return true;

        else return false;
    }

    int UpdatePlayerScore(int inPlayerScore, TextMeshProUGUI scoreText)
    {
        int outPlayerScore = inPlayerScore + DeltaScore(roundTimer);

        scoreText.text = outPlayerScore.ToString();

        return outPlayerScore;
    }

    int DeltaScore(float correctAnswerTime = 0.1f, int attempts = 1)
    {
        float fDeltaScore = 100 * (attempts / correctAnswerTime);

        int deltaScore = (int)fDeltaScore;

        return deltaScore;
    }

    void GoToNextPrompt()
    {
        
    }

    void HandlePlayerCorrect(GameObject playerGO)
    {
        if (!isBetweenRounds)
        {
            nextRoundTimer = 0;
            isBetweenRounds = true;
        }

        if (playerGO)
        {
            if (playerGO == p1ColorInputGO)
            {

            }

            if (playerGO == p2ColorInputGO)
            {

            }
        }
    }

    void StartNewRound()
    {
        isBetweenRounds = false;
        roundTimer = 0;

        UpdateColor(colorPromptGO);
        PlayerController.Instance.DisplayRandomColor();

        OnPromptUpdated?.Invoke();
        Debug.Log("Call Event: " + OnPromptUpdated + ".");
    }
}
