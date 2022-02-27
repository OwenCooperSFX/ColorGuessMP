using System.Collections.Generic;
using UnityEngine;

public enum ButtonInput { Up, Down, Left, Right, invalid }

public class PlayerController_new : MonoBehaviour
{
    [SerializeField] private Control _upControl, _leftControl, _downControl, _rightControl;

    public Control UpControl => _upControl;
    public Control LeftControl => _leftControl;
    public Control DownControl => _downControl;
    public Control RightControl => _rightControl;

    private List<Control> _controls = new List<Control>();
    public List<Control> Controls => _controls;

    [SerializeField] private List<Color> _colors = new List<Color>();
    private List<Color> _lastColorOrder = new List<Color>();

    private List<Color> _colorAssignments = new List<Color>();
    public List<Color> ColorAssignments => _colorAssignments;

    // TODO: Fix color rendering. Separate color logic into separate component so that this class only handles inputs.
    // New component can respond to raised input events.

    private void Awake()
    {
        InitializeControls();
        InitializeColors();
    }

    private void Update()
    {
        if (Input.anyKey)
            GetInput();
    }

    private void OnEnable()
    {
        EventManager.OnButtonInput += HandleButtonInput;
    }

    private void OnDisable()
    {
        EventManager.OnButtonInput -= HandleButtonInput;
    }

    void InitializeColors()
    {
        _colors.Add(Color.red);
        _colors.Add(Color.green);
        _colors.Add(Color.blue);
        _colors.Add(Color.yellow);

        _lastColorOrder = _colors;
        _colorAssignments = _colors;

        AssignControlColors();
    }

    void InitializeControls()
    {
        _controls.Add(_upControl);
        _controls.Add(_leftControl);
        _controls.Add(_downControl);
        _controls.Add(_rightControl);
    }

    public ColorOption DoInput(ButtonInput buttonInput)
    {
        EventManager.RaiseButtonInput(buttonInput);

        return ColorOption.invalid;
    }

    private ColorOption GetInput()
    {
        ButtonInput buttonInput = ButtonInput.invalid;

        if (Input.GetKeyDown(_upControl.Button))
        {
            // Do top color
            buttonInput = ButtonInput.Up;
        }
        if (Input.GetKeyDown(_leftControl.Button))
        {
            // Do left color
            buttonInput = ButtonInput.Left;
        }
        if (Input.GetKeyDown(_rightControl.Button))
        {
            // Do bottom color
            buttonInput = ButtonInput.Right;
        }
        if (Input.GetKeyDown(_downControl.Button))
        {
            // Do right color
            buttonInput = ButtonInput.Down;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            AssignControlColors();

        return DoInput(buttonInput);
    }

    void HandleButtonInput(ButtonInput buttonInput)
    {
        var control = new Control();

        switch (buttonInput)
        {
            case ButtonInput.Up:
                control = _upControl;
                break;
            case ButtonInput.Left:
                control = _leftControl;
                break;
            case ButtonInput.Down:
                control = _downControl;
                break;
            case ButtonInput.Right:
                control = _rightControl;
                break;
        }

        if (control.ColorObjectNew)
            control.ColorObjectNew.Pressed();
    }

    List<Color> ShuffleColors(List<Color> colorList)
    {
        List<Color> assignments = colorList;

        int colorsCount = _colors.Count;

        for (int i = 0; i < colorsCount; i++)
        {
            Color color = _colors[i];
            int rndInt = Random.Range(0, i);

            assignments[i] = _colors[rndInt];
            assignments[rndInt] = color;
        }

        return assignments;
    }

    void AssignControlColors()
    {
        // logic for shuffling player control colors. Avoids repeating pattern most of the time (~85% different from last).
        // TODO: implement true avoid-last-pattern logic.

        _colorAssignments = ShuffleColors(_colors);

        if (_colorAssignments == _lastColorOrder)
        {
            _colorAssignments = ShuffleColors(_colors);
            _lastColorOrder = _colorAssignments;
        }

        //Rendering
        for (int i = 0; i < _controls.Count; i++)
        {
            ColorObject_new colorObjectNew = _controls[i].ColorObjectNew;
            Material material = colorObjectNew.GetComponent<MeshRenderer>().material;

            material.color = _colorAssignments[i];
            colorObjectNew.CurrentColor = GetColorOptionFromMaterialColor(colorObjectNew);
        }
    }

    public ColorOption GetColorOptionFromMaterialColor(ColorObject_new colorObjectNew)
    {
        // When player inputs a control, check its color and assign it a ColorOptions value,
        // TBD: Used for comparing against colorPrompt.

        Color color = colorObjectNew.GetComponent<MeshRenderer>().material.color;
        colorObjectNew.CurrentColor = ColorOption.invalid;

        if (color == Color.red)
            colorObjectNew.CurrentColor = ColorOption.red;
        if (color == Color.blue)
            colorObjectNew.CurrentColor = ColorOption.blue;
        if (color == Color.yellow)
            colorObjectNew.CurrentColor = ColorOption.yellow;
        if (color == Color.green)
            colorObjectNew.CurrentColor = ColorOption.green;

        return colorObjectNew.CurrentColor;
    }

    [System.Serializable]
    public struct Control
    {
        [SerializeField] ButtonInput _assignment;
        public ButtonInput Assignment => _assignment;
        [SerializeField] KeyCode _button;
        public KeyCode Button => _button;
        [SerializeField] ColorObject_new _colorObject;
        public ColorObject_new ColorObjectNew => _colorObject;
    }
}
