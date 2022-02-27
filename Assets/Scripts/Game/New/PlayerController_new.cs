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

    public List<Control> Controls { get; } = new List<Control>();

    [SerializeField] private List<Color> _colors = new List<Color>();
    private List<Color> _lastColorOrder = new List<Color>();

    public List<Color> ColorAssignments { get; private set; } = new List<Color>();

    // Separate color logic into separate component so that this class only handles inputs.
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
        ColorAssignments = _colors;

        AssignControlColors();
    }

    void InitializeControls()
    {
        Controls.Add(_upControl);
        Controls.Add(_leftControl);
        Controls.Add(_downControl);
        Controls.Add(_rightControl);
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
            buttonInput = ButtonInput.Up;
        }
        if (Input.GetKeyDown(_leftControl.Button))
        {
            buttonInput = ButtonInput.Left;
        }
        if (Input.GetKeyDown(_rightControl.Button))
        {
            buttonInput = ButtonInput.Right;
        }
        if (Input.GetKeyDown(_downControl.Button))
        {
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

    List<Color> ShuffleColors()
    {
        List<Color> assignments = _colors;

        for (int i = 0; i < assignments.Count; i++)
        {
            Color color = assignments[i];
            int rndInt = Random.Range(0, i);

            assignments[i] = assignments[rndInt];
            assignments[rndInt] = color;
        }

        return assignments;
    }

    void AssignControlColors()
    {
        ColorAssignments = ShuffleColors();

        if (ColorAssignments == _lastColorOrder)
        {
            ColorAssignments = ShuffleColors();
            _lastColorOrder = ColorAssignments;
        }

        RenderMaterialColors();
    }

    private void RenderMaterialColors()
    {
        for (int i = 0; i < Controls.Count; i++)
        {
            ColorObject_new colorObjectNew = Controls[i].ColorObjectNew;

            colorObjectNew.UpdateCurrentColor(ColorAssignments[i]);
        }
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
