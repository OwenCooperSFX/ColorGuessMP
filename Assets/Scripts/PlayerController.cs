using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum InputButton { Up, Down, Left, Right, invalid }

public class PlayerController : ColorObject
{
    public static PlayerController Instance;

    [Header("Player control directions")]
    public List<ColorObject> p1_controls;
    public List<ColorObject> p2_controls;

    private List<Color> colors;
    private List<Color> lastColorOrder;

    public List<Color> colorAssignments;

    [Header("Button Animation properties")]
    [SerializeField] private float inputAnimTime = 0.2f;
    [SerializeField] private float inputAnimDepth = 0.2f;

    // Events
    public delegate InputButton P1Input(InputButton inputSelection);
    public event P1Input OnP1Input;

    public delegate InputButton P2Input(InputButton inputSelection);
    public event P2Input OnP2Input;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        colors = new List<Color>();

        // Make color options pool (list)
        colors.Add(Color.red);
        colors.Add(Color.green);
        colors.Add(Color.blue);
        colors.Add(Color.yellow);

        colorAssignments = colors;
        lastColorOrder = colorAssignments;
    }

    void Update()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public ColorOptions InitialSpaceInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return DisplayRandomColor();
        }

        else return ColorOptions.invalid;
    }

    public ColorOptions GetPlayer1Input()
    {
        if (GameManager.Instance.p1InputEnabled)
        {
            // Player 1 -- WASD
            if (Input.GetKeyDown(KeyCode.W))
            {
                // Do top color
                P1InputDown(InputButton.Up);
                
                return GetColor(p1_controls[0]);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                // Do left color
                P1InputDown(InputButton.Left);
                return GetColor(p1_controls[1]);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                // Do bottom color
                P1InputDown(InputButton.Down);
                return GetColor(p1_controls[3]);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                // Do right color
                P1InputDown(InputButton.Right);
                return GetColor(p1_controls[2]);
            }
        }

        return ColorOptions.invalid;
    }

    public ColorOptions GetPlayer2Input()
    {
        if (GameManager.Instance.p2InputEnabled)
        {
            // Player 2 -- IJKL
            if (Input.GetKeyDown(KeyCode.I))
            {
                // Do top color
                P2InputDown(InputButton.Up);
                return GetColor(p2_controls[0]);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                // Do left color
                P2InputDown(InputButton.Left);
                return GetColor(p2_controls[1]);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                // Do bottom color
                P2InputDown(InputButton.Down);
                return GetColor(p2_controls[3]);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                // Do right color
                P2InputDown(InputButton.Right);
                return GetColor(p2_controls[2]);
            }
        }

        return ColorOptions.invalid;
    }

    public ColorOptions DisplayRandomColor()
    {
        AssignControlColors(p1_controls);
        AssignControlColors(p2_controls);

        return ColorOptions.random;
    }

    public InputButton P1InputDown(InputButton inputButton)
    {
        StartCoroutine(AnimateButtonDown(inputButton, p1_controls));

        OnP1Input?.Invoke(inputButton);

        return inputButton;
    }

    public InputButton P2InputDown(InputButton inputButton)
    {
        StartCoroutine(AnimateButtonDown(inputButton, p2_controls));

        OnP2Input?.Invoke(inputButton);

        return inputButton;
    }

    void Init()
    {
        // Singleton logic
        Instance = FindObjectOfType<PlayerController>();

        if (Instance && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    List<Color> ShuffleColors(List<Color> in_colorList)
    {
        List<Color> assignments = in_colorList;

        int colorsCount = colors.Count;

        for (int index = 0; index < colorsCount; index++)
        {
            Color color = colors[index];
            int rndInt = Random.Range(0, index);
            assignments[index] = colors[rndInt];
            assignments[rndInt] = color;
        }

        return assignments;
    }

    void AssignControlColors(List<ColorObject> playerControls)
    {
        // logic for shuffling player control colors. Avoids repeating pattern most of the time (~85% different from last).
        // TODO: implement true avoid-last-pattern logic.

        colorAssignments = ShuffleColors(colors);

        if (colorAssignments == lastColorOrder)
        {
            colorAssignments = ShuffleColors(colors);
            lastColorOrder = colorAssignments;
        }

        //

        if (playerControls == p1_controls)
        {
            for (int i = 0; i < p1_controls.Count; i++)
            {
                Material material = p1_controls[i].GetComponent<MeshRenderer>().material;
                material.color = colorAssignments[i];
            }
        }

        if (playerControls == p2_controls)
        {
            for (int i = 0; i < p2_controls.Count; i++)
            {
                Material material = p2_controls[i].GetComponent<MeshRenderer>().material;
                material.color = colorAssignments[i];
            }
        }
    }

    public ColorOptions GetColor(ColorObject playerControlInput)
    {
        // When player inputs a control, check its color and assign it a ColorOptions value,
        // TBD: Used for comparing against colorPrompt.

        Color color = playerControlInput.GetComponent<MeshRenderer>().material.color;
        playerControlInput.currentColor = ColorOptions.invalid;

        if (color == Color.red)
            playerControlInput.currentColor = ColorOptions.red;
        if (color == Color.blue)
            playerControlInput.currentColor = ColorOptions.blue;
        if (color == Color.yellow)
            playerControlInput.currentColor = ColorOptions.yellow;
        if (color == Color.green)
            playerControlInput.currentColor = ColorOptions.green;

        if (!GameManager.Instance.isBetweenRounds)
            playerControlInput.FlashButtonLight();

        return playerControlInput.currentColor;
    }

    IEnumerator AnimateButtonDown(InputButton inputButton, List<ColorObject> playerControlList)
    {
        //TODO: animate input object on player input for visual feedback.
        //TODO: Use DOTween for these for better animation.
        Vector3 transformPosDelta = new Vector3(0, 0, inputAnimDepth);

        int buttonIndex;

        switch (inputButton)
        {
            case InputButton.Up:
                buttonIndex = 0;
                break;
            case InputButton.Left:
                buttonIndex = 1;
                break;
            case InputButton.Down:
                buttonIndex = 3;
                break;
            case InputButton.Right:
                buttonIndex = 2;
                break;
            default:
                buttonIndex = 0;
                break;
        }

        playerControlList[buttonIndex].transform.position += transformPosDelta;

        yield return new WaitForSeconds(inputAnimTime);

        playerControlList[buttonIndex].transform.position -= transformPosDelta;
    }
}
