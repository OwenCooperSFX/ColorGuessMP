using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

/*
NOTES:
    TODO:
    - Get event system working                      DONE

    TODO:
    - automate top prompt random color              DONE
    - give players color control layout             DONE
    - reward score for matching color               DONE      
    - refresh random color for prompt               DONE
    - re-randomize the winning player's controls    IN PROGRESS

    TODO:
    - Animations for player input                   DONE
    - Animations for new random color prompt
    - Animations for reward/penalty                 DONE

    TODO:
    - Winner screen
    - Tutorial

    TODO:
    - Sound design, music
    
    TODO:
    - Use InputEvents to get away from using the Update method here...
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
    private float newTimeBetweenRounds = 3f;
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

    private Vector3 _colorPromptStartScale;
    public Vector3 colorPromptStartScale { get { return _colorPromptStartScale; } }

    private Quaternion _colorPromptStartRotation;
    public Quaternion colorPromptStartRotation { get { return _colorPromptStartRotation; } }

    public delegate void ObjectInitialized();
    public event ObjectInitialized OnGameManagerInitialized;

    private void OnEnable()
    {
        EventManager.OnPlayerInputCorrect += HandlePlayerCorrect;
        EventManager.OnPlayerInputWrong += HandlePlayerIncorrect;

        EventManager.OnP1Input += HandleP1Input;
        EventManager.OnP2Input += HandleP2Input;

        EventManager.OnExplosionStarted += ResetGame;
        EventManager.OnExplosionFinished += StartFirstRound;

        OnGameManagerInitialized += StartFirstRound;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerInputCorrect -= HandlePlayerCorrect;
        EventManager.OnPlayerInputWrong -= HandlePlayerIncorrect;

        EventManager.OnP1Input -= HandleP1Input;
        EventManager.OnP2Input -= HandleP2Input;

        EventManager.OnExplosionStarted -= ResetGame;
        EventManager.OnExplosionFinished -= StartFirstRound;

        OnGameManagerInitialized -= StartFirstRound;
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
        ResetGame();

        // Cache initial transform values
        _colorPromptStartScale = colorPromptGO.transform.localScale;
        _colorPromptStartRotation = colorPromptGO.transform.localRotation;
    }

    void Start()
    {
        if (OnGameManagerInitialized != null)
            OnGameManagerInitialized();
    }

    IEnumerator StartNewRoundWithDelay(float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);
        StartNewRound();
    }

    void StartFirstRound()
    {
        ResetGame();
        StartCoroutine(StartNewRoundWithDelay());
    }

    void Update()
    {
        if (_isBetweenRounds)
        {
            if (nextRoundTimer < newTimeBetweenRounds)
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
        //if (PlayerController.Instance.InitialSpaceInput() != ColorOption.invalid)
        //{
        //    StartNewRound();
        //}

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
                    EventManager.RaisePlayerInputCorrect(p1ColorInputGO);
                    //OnPlayerInputCorrect?.Invoke(p1ColorInputGO);
                    //Debug.Log("Call Event: " + OnPlayerInputCorrect + " (" + p1ColorInputGO.name + ").");
                }
                else
                {
                    // Player 1 is wrong
                    EventManager.RaisePlayerInputWrong(p1ColorInputGO);
                    //OnPlayerInputIncorrect?.Invoke(p1ColorInputGO);
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
                    EventManager.RaisePlayerInputCorrect(p2ColorInputGO);
                    //OnPlayerInputCorrect?.Invoke(p2ColorInputGO);
                    //Debug.Log("Call Event: " + OnPlayerInputCorrect + " (" + p2ColorInputGO.name + ").");
                }
                else
                {
                    // Player 2 is wrong
                    EventManager.RaisePlayerInputWrong(p2ColorInputGO);
                    //OnPlayerInputIncorrect?.Invoke(p2ColorInputGO);
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
        if (Instance && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    bool  InputEqualsPrompt(ColorOption input)
    {
        ColorOption promptColor = colorPromptGO.GetComponent<ColorObject>().currentColor;

        if (input == promptColor)
            return true;

        else return false;
    }

    private void AnimateScorePopup(GameObject scorePopup, int deltaScore)
    {
        scorePopup.SetActive(false);
        TextMeshProUGUI scorePopupText = scorePopup.GetComponent<TextMeshProUGUI>();
        scorePopupText.text = OCMath.RoundFloat(roundTimer, 3).ToString(); //deltaScore.ToString();

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

            colorPromptGO.transform.DOKill();            
        }

        float colorPromptSpinDir = 1f;
        float colorPromptSpinSpd = 5f;

        if (playerGO)
        {
            if (playerGO == p1ColorInputGO)
            {
                AnimateScorePopup(p1ScorePopup, DeltaScore(roundTimer, p1Attempts));

                p1Score = UpdatePlayerScore(p1Score, p1Attempts, p1ScoreText);

                TugOfWar.Instance.MoveBadThing(TugOfWar.Instance.pushAmount * (.001f * DeltaScore(roundTimer, p1Attempts)), newTimeBetweenRounds);

                colorPromptSpinDir = -1;
                colorPromptSpinSpd *= 100/(float)DeltaScore(roundTimer, p1Attempts);
            }

            if (playerGO == p2ColorInputGO)
            {
                AnimateScorePopup(p2ScorePopup, DeltaScore(roundTimer, p2Attempts));

                p2Score = UpdatePlayerScore(p2Score, p2Attempts ,p2ScoreText);

                TugOfWar.Instance.MoveBadThing(-TugOfWar.Instance.pushAmount * (.001f * DeltaScore(roundTimer, p2Attempts)), newTimeBetweenRounds);

                colorPromptSpinDir = 1;
                colorPromptSpinSpd *= 100/(float)DeltaScore(roundTimer, p2Attempts);
            }

            colorPromptGO.transform.DOKill();
            colorPromptGO.transform.localScale = _colorPromptStartScale;
            colorPromptGO.transform.localRotation = _colorPromptStartRotation;

            // TODO: use events to simplify this.
            // Set the speed of the colorPrompt spinning tween.
            // Scale Tween speed based on player reaction time.
            colorPromptSpinDir *= 180f;
            colorPromptGO.transform.DOLocalRotate(new Vector3(0f, 0f, colorPromptSpinDir), colorPromptSpinSpd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
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

        newTimeBetweenRounds = timeBetweenRounds - Mathf.Abs(.5f * TugOfWar.Instance.badThing_xPos);

        UpdateColor(colorPromptGO);

        colorPromptGO.transform.DOKill();
        colorPromptGO.transform.localScale = _colorPromptStartScale;
        colorPromptGO.transform.localRotation = _colorPromptStartRotation;
        colorPromptGO.transform.DOPunchScale(new Vector3(.2f, .2f, 1f), .5f, 10, .5f);

        // Set the speed or the colorPrompt pulsing tween.
        // TODO: use events to simplify this.
        if (Mathf.Abs(TugOfWar.Instance.badThing_xPos) >= TugOfWar.Instance.horizontalLimit)
        {
            // Go back to default speed if boundary was exceeded.
            colorPromptGO.transform.DOScale(colorPromptGO.transform.localScale * 1.1f, .3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
        else
        {
            // Scale Tween speed based on BadThing position if boundary not exceeded.
            colorPromptGO.transform.DOScale(colorPromptGO.transform.localScale * 1.1f, (Mathf.Clamp(.5f / Mathf.Abs(TugOfWar.Instance.badThing_xPos), .01f, .3f)))
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        PlayerController.Instance.DisplayRandomColor();

        EventManager.RaisePromptUpdated();
    }

    void IncreaseP1Attempts()
    {
        if (p1Attempts > 2)
        {
            EventManager.RaisePlayerInputWrong(p1ColorInputGO);
            // OnPlayerInputIncorrect?.Invoke(p1ColorInputGO);
            _p1InputEnabled = false;
        }
        else if (!_isBetweenRounds)
            p1Attempts++;
    }

    void IncreaseP2Attempts()
    {
        if (p2Attempts > 2)
        {
            EventManager.RaisePlayerInputWrong(p1ColorInputGO);
            // OnPlayerInputIncorrect?.Invoke(p2ColorInputGO);
            _p2InputEnabled = false;
        }
        else if (!_isBetweenRounds)
            p2Attempts++;
    }

    private InputButton HandleP2Input(InputButton in_Button)
    {
        //TODO: Use this to get away from handling things in Update.
        return InputButton.invalid;
    }

    private InputButton HandleP1Input(InputButton in_Button)
    {
        //TODO: Use this to get away from handling things in Update.
        return InputButton.invalid;
    }

    public void ResetGame()
    {
        // Clear timers
        roundTimer = roundTimeLimit;
        nextRoundTimer = timeBetweenRounds;
        newTimeBetweenRounds = timeBetweenRounds;
    }

}
