using UnityEngine;
using TMPro;
using DG.Tweening;

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

public enum ColorOption { invalid, red, blue, yellow, green, random }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public ColorOption colorOption;

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
        if (PlayerController.Instance.InitialSpaceInput() != ColorOption.invalid)
        {
            StartNewRound();
        }

        // Don't continue if we are between rounds
        if (_isBetweenRounds)
            return;

        // Handle player inputs
        if (PlayerController.Instance.GetPlayer1Input() != ColorOption.invalid)
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


        if (PlayerController.Instance.GetPlayer2Input() != ColorOption.invalid)
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
        ColorOption inputColor = 0;
        ColorOption newColorOption = 0;
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
            case ColorOption.blue:
                newColor = Color.blue;
                break;
            case ColorOption.green:
                newColor = Color.green;
                break;
            case ColorOption.red:
                newColor = Color.red;
                break;
            case ColorOption.yellow:
                newColor = Color.yellow;
                break;
            case ColorOption.invalid:
                Debug.LogWarning("Invalid color!");
                break;
        }

        if (targetMaterial.color != newColor)
            targetMaterial.color = newColor;
    }

    public ColorOption RandomizeColor()
    {
        int rndInt = UnityEngine.Random.Range(1, 5);

        ColorOption rndColor = (ColorOption)rndInt;

        return rndColor;
    }

    void Init()
    {
        // Singleton logic

        if (!Instance)
        {
            Instance = FindObjectOfType<GameManager>();
        }
        
        if (Instance && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    bool  InputEqualsPrompt(ColorOption input)
    {
        ColorOption promptColor = colorPromptGO.GetComponent<ColorObject>().currentColor;

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
        scorePopupText.text = RoundFloat(roundTimer, 3).ToString();//deltaScore.ToString();

        if (deltaScore == 0)
            scorePopupText.color = Color.red;
        else
            scorePopupText.color = Color.green;

        scorePopup.SetActive(true);
    }

    /// <summary>
    /// Rounds to the closest decimal place, defined by value of decimalPlaces.
    /// decimalPlaces must be a positive value less than or equal to 10.
    /// </summary>
    float RoundFloat(float inFloat, int decimalPlaces = 1)
    {
        #region Error handling
        if (decimalPlaces > 10)
        {
            Debug.LogWarning("10 is the maximum decimal places allowed.");

            decimalPlaces = 10;
        }
        else if (decimalPlaces <= 0)
        {
            Debug.LogError("decimalPlaces argument must be a positive value.");
        }
        #endregion

        int factor = 10;

        for (int i = 1; i < decimalPlaces; i++)
        {
            factor *= 10;
        }

        inFloat = Mathf.Round(inFloat * factor) / factor;

        return inFloat;
    }

    void HandlePlayerCorrect(GameObject playerGO)
    {
        if (_isBetweenRounds)
            return;
        else
        {
            nextRoundTimer = 0;
            _isBetweenRounds = true;

            colorPromptGO.transform.DOKill();
            colorPromptGO.transform.DOScale(colorPromptGO.transform.localScale * 1.1f, .3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        if (playerGO)
        {
            if (playerGO == p1ColorInputGO)
            {
                AnimateScorePopup(p1ScorePopup, DeltaScore(roundTimer, p1Attempts));

                p1Score = UpdatePlayerScore(p1Score, p1Attempts, p1ScoreText);

                TugOfWar.Instance.MoveBadThing(TugOfWar.Instance.pushAmount/roundTimer, TugOfWar.Instance.pushSpeed);
            }

            if (playerGO == p2ColorInputGO)
            {
                AnimateScorePopup(p2ScorePopup, DeltaScore(roundTimer, p2Attempts));

                p2Score = UpdatePlayerScore(p2Score, p2Attempts ,p2ScoreText);

                TugOfWar.Instance.MoveBadThing(-TugOfWar.Instance.pushAmount / roundTimer, TugOfWar.Instance.pushSpeed);
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

        colorPromptGO.transform.DOKill();
        colorPromptGO.transform.DOPunchScale(new Vector3(.2f, .2f, 1f), .5f, 10, .5f);

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
