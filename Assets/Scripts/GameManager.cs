using UnityEngine;
using TMPro;

/*
NOTES:
    TODO:
    - Get event system working                      DONE

    TODO:
    - automate top prompt random color              
    - give players color control layout             DONE
    - reward score for matching color               DONE      
    - refresh random color for prompt               DONE
    - re-randomize the winning player's controls    IN PROGRESS

    TODO:
    - Animations for player input                   DONE
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

    public bool isBetweenRounds { get { return _isBetweenRounds; } }
    private bool _isBetweenRounds = false;

    [Header("Scoring: Player 1")]
    [SerializeField] private TextMeshProUGUI p1ScoreText = null;
    [SerializeField] private GameObject p1ScorePopup = null;
    public int p1Score = 0000;
    public int p1Attempts = 0;
    public float p1CorrectAnswerTime = 0f;
    [HideInInspector] public bool p1InputEnabled { get { return _p1InputEnabled; } }
    private bool _p1InputEnabled = true;

    [Header("Scoring: Player 2")]
    [SerializeField] private TextMeshProUGUI p2ScoreText = null;
    [SerializeField] private GameObject p2ScorePopup = null;
    public int p2Score = 0000;
    public int p2Attempts = 0;
    public float p2CorrectAnswerTime = 0f;
    [HideInInspector] public bool p2InputEnabled { get { return _p2InputEnabled; } }
    private bool _p2InputEnabled = true;

    // Events
    public delegate void PromptUpdated();
    public event PromptUpdated OnPromptUpdated;

    public delegate void PlayerInputCorrect(GameObject playerInputGO);
    public event PlayerInputCorrect OnPlayerInputCorrect;

    public delegate void PlayerInputIncorrect(GameObject playerInputGO);
    public event PlayerInputIncorrect OnPlayerInputIncorrect;

    private void OnEnable()
    {
        OnPlayerInputCorrect += HandlePlayerCorrect;
        OnPlayerInputIncorrect += HandlePlayerIncorrect;
    }

    private void OnDisable()
    {
        OnPlayerInputCorrect -= HandlePlayerCorrect;
        OnPlayerInputIncorrect -= HandlePlayerIncorrect;
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
        if (_isBetweenRounds)
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
        else if (!_p1InputEnabled && !_p2InputEnabled)
        {
            nextRoundTimer = 0;
            AudioManager.Instance.HandlePromptUpdated();
            AnimateScorePopup(p1ScorePopup, 0);
            AnimateScorePopup(p2ScorePopup, 0);
            _isBetweenRounds = true;
        }

        if (roundTimer < roundTimeLimit)
            roundTimer += Time.deltaTime;

        //temp setup to get things going manually
        if (PlayerController.Instance.InitialSpaceInput() != ColorOptions.invalid)
        {
            StartNewRound();
        }

        // Don't continue if we are between rounds
        if (_isBetweenRounds)
            return;

        // Handle player inputs
        if (PlayerController.Instance.GetPlayer1Input() != ColorOptions.invalid)
        {
            if (_isBetweenRounds)
                return;

            IncreaseP1Attempts();

            if (_p1InputEnabled)
            {
                if (InputEqualsPrompt(PlayerController.Instance.GetPlayer1Input()))
                {
                    // Player 1 is correct
                    OnPlayerInputCorrect?.Invoke(p1ColorInputGO);
                    //Debug.Log("Call Event: " + OnPlayerInputCorrect + " (" + p1ColorInputGO.name + ").");
                }
                else
                {
                    // Player 1 is wrong
                    OnPlayerInputIncorrect?.Invoke(p1ColorInputGO);
                    //Debug.Log("Call Event: " + OnPlayerInputIncorrect + " (" + p1ColorInputGO.name + ").");
                }
            }

        }


        if (PlayerController.Instance.GetPlayer2Input() != ColorOptions.invalid)
        {
            if (_isBetweenRounds)
                return;

            IncreaseP2Attempts();

            if (_p2InputEnabled)
            {
                if (InputEqualsPrompt(PlayerController.Instance.GetPlayer2Input()))
                {
                    // Player 2 is correct
                    OnPlayerInputCorrect?.Invoke(p2ColorInputGO);
                    //Debug.Log("Call Event: " + OnPlayerInputCorrect + " (" + p2ColorInputGO.name + ").");
                }
                else
                {
                    // Player 2 is wrong
                    OnPlayerInputIncorrect?.Invoke(p2ColorInputGO);
                    //Debug.Log("Call Event: " + OnPlayerInputIncorrect + " (" + p2ColorInputGO.name + ").");
                }
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

    void GoToNextPrompt()
    {
        
    }

    private void AnimateScorePopup(GameObject scorePopup, int deltaScore)
    {
        scorePopup.SetActive(false);
        TextMeshProUGUI scorePopupText = scorePopup.GetComponent<TextMeshProUGUI>();
        scorePopupText.text = deltaScore.ToString();

        if (deltaScore == 0)
            scorePopupText.color = Color.red;
        else
            scorePopupText.color = Color.green;

        scorePopup.SetActive(true);
    }

    void HandlePlayerCorrect(GameObject playerGO)
    {
        if (_isBetweenRounds)
            return;
        else
        {
            nextRoundTimer = 0;
            _isBetweenRounds = true;
        }

        if (playerGO)
        {
            if (playerGO == p1ColorInputGO)
            {
                AnimateScorePopup(p1ScorePopup, DeltaScore(roundTimer, p1Attempts));

                p1Score = UpdatePlayerScore(p1Score, p1Attempts, p1ScoreText);
            }

            if (playerGO == p2ColorInputGO)
            {
                AnimateScorePopup(p2ScorePopup, DeltaScore(roundTimer, p2Attempts));

                p2Score = UpdatePlayerScore(p2Score, p2Attempts ,p2ScoreText);
            }
        }
    }

    void HandlePlayerIncorrect(GameObject playerGO)
    {
        if (_isBetweenRounds)
            return;

        if (playerGO)
        {
            if (playerGO == p1ColorInputGO)
                AnimateScorePopup(p1ScorePopup, 0);

            if (playerGO == p2ColorInputGO)
                AnimateScorePopup(p2ScorePopup, 0);
        }
    }

    int UpdatePlayerScore(int inPlayerScore, int inAttempts, TextMeshProUGUI scoreText)
    {
        int outPlayerScore = inPlayerScore + DeltaScore(roundTimer, inAttempts);

        scoreText.text = outPlayerScore.ToString();

        return outPlayerScore;
    }

    int DeltaScore(float correctAnswerTime, int attempts)
    {
        // TODO: fix broken multiplier switching

        int multiplier = 0;
        switch (attempts)
        {
            case 1:
                multiplier = 1;
                break;
            case 2:
                multiplier = 3;
                break;
            case 3:
                multiplier = 6;
                break;
            default:
                // for any invalid values.
                return 0;
        }

        float fDeltaScore = 2000 / ((2 * correctAnswerTime) + (attempts * multiplier));

        int deltaScore = (int)fDeltaScore;

        // print(multiplier);

        return deltaScore;
    }

    void StartNewRound()
    {
        _p1InputEnabled = true;
        _p2InputEnabled = true;

        _isBetweenRounds = false;

        p1Attempts = 0;
        p2Attempts = 0;

        roundTimer = 0;

        UpdateColor(colorPromptGO);
        PlayerController.Instance.DisplayRandomColor();

        OnPromptUpdated?.Invoke();
        Debug.Log("Call Event: " + OnPromptUpdated + ".");
    }

    void IncreaseP1Attempts()
    {
        if (p1Attempts > 2)
        {
            OnPlayerInputIncorrect?.Invoke(p1ColorInputGO);
            _p1InputEnabled = false;
        }
        else if (!_isBetweenRounds)
            p1Attempts++;
    }

    void IncreaseP2Attempts()
    {
        if (p2Attempts > 2)
        {
            OnPlayerInputIncorrect?.Invoke(p2ColorInputGO);
            _p2InputEnabled = false;
        }
        else if (!_isBetweenRounds)
            p2Attempts++;
    }
}
