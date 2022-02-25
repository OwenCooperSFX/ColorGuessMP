using System.Collections.Generic;
using UnityEngine;

public enum ButtonInput { Up, Down, Left, Right, invalid }

public class PlayerController_new : MonoBehaviour
{
    [SerializeField] private Control _upControl, _leftControl, _downControl, _rightControl;
    //public List<ColorObject_new> Controls { get => _controls; }
    public Control UpControl => _upControl;
    public Control LeftControl => _leftControl;
    public Control DownControl => _downControl;
    public Control RightControl => _rightControl;

    private List<Color> _colors = new List<Color>();
    private List<Color> _lastColorOrder = new List<Color>();
    public List<Color> ColorAssignments => _colors;

    private void Awake()
    {
        InitializeColors();
    }

    private void Update()
    {
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
    }

    public ColorOption DoInput(ButtonInput buttonInput)
    {
        EventManager.RaiseButtonInput(buttonInput);

        return ColorOption.invalid;
    }

    private ColorOption GetInput()
    {
        if (Input.GetKeyDown(_upControl.Button))
        {
            // Do top color
            return DoInput(ButtonInput.Up);
        }
        if (Input.GetKeyDown(_leftControl.Button))
        {
            // Do left color
            return DoInput(ButtonInput.Left);
        }
        if (Input.GetKeyDown(_rightControl.Button))
        {
            // Do bottom color
            return DoInput(ButtonInput.Down);
        }
        if (Input.GetKeyDown(_downControl.Button))
        {
            // Do right color
            return DoInput(ButtonInput.Right);
        }

        // No input - return no color. 
        return ColorOption.invalid;
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

        control.ColorObjectNew.Pressed();
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
