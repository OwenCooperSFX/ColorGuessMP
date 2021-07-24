using System.Collections.Generic;
using UnityEngine;

public enum InputSelections { Up, Down, Left, Right, invalid }

public class PlayerController : ColorObject
{
    public static PlayerController Instance;

    [Header("Player control directions")]
    public List<ColorObject> p1_controls;
    public List<ColorObject> p2_controls;

    private List<Color> colors;
    private List<Color> lastColorOrder;

    public List<Color> colorAssignments;

    private void OnEnable()
    {
        GameManager.Instance.OnP1Input += OnP1Input;
        GameManager.Instance.OnP2Input += OnP2Input;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnP1Input -= OnP1Input;
        GameManager.Instance.OnP2Input -= OnP2Input;
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

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ColorOptions GetPlayer1Input()
    {
        // Player 1 -- WASD
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Do top color
            OnP1Input();
            return GetColor(p1_controls[0]);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Do left color
            OnP1Input();
            return GetColor(p1_controls[1]);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Do bottom color
            OnP1Input();
            return GetColor(p1_controls[3]);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Do right color
            OnP1Input();
            return GetColor(p1_controls[2]);
        }

        return ColorOptions.invalid;
    }

    public ColorOptions GetPlayer2Input()
    {
        // Player 2 -- IJKL
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Do top color
            OnP2Input();
            return GetColor(p2_controls[0]);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            // Do left color
            OnP2Input();
            return GetColor(p2_controls[1]);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Do bottom color
            OnP2Input();
            return GetColor(p2_controls[3]);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Do right color
            OnP2Input();
            return GetColor(p2_controls[2]);
        }

        return ColorOptions.invalid;
    }

    public ColorOptions DisplayRandomColor()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AssignControlColors(p1_controls);
            AssignControlColors(p2_controls);

            return ColorOptions.random;
        }
        
        return ColorOptions.invalid;
    }

    public void OnP1Input()
    {

    }

    public void OnP2Input()
    {

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

        playerControlInput.FlashButtonLight();

        return playerControlInput.currentColor;
    }

    void AnimateInputObject(ColorObject inputObject)
    {
        //TODO: animate input object on player input for visual feedback.

        //Animator animator = inputObject.GetComponent<Animator>();

        //if (!animator)
        //    Debug.LogWarning("No animator component! " + inputObject);

        //animator.Play("InputSwell");
    }
}
