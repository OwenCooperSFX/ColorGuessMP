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

    private ControlColorHandler _controlColorHandler;


    private void Awake()
    {
        InitializeControls();
        InitializeControlColorHandler();
    }

    private void Update()
    {
        if (Input.anyKey)
            GetInput();
    }

    private void InitializeControls()
    {
        Controls.Add(_upControl);
        Controls.Add(_leftControl);
        Controls.Add(_downControl);
        Controls.Add(_rightControl);

        foreach (var control in Controls)
        {
            control.ColorObjectNew.ButtonAssignment = control.InputAssignment;
        }
    }

    private void InitializeControlColorHandler()
    {
        if (!_controlColorHandler)
        {
            _controlColorHandler = GetComponent<ControlColorHandler>();
            if (!_controlColorHandler)
            {
                _controlColorHandler = new ControlColorHandler();
            }
        }

        _controlColorHandler.AssignControlColors();
        _controlColorHandler.RenderMaterialColors(Controls);
    }

    public void DoInput(ButtonInput buttonInput)
    {
        EventManager.RaiseButtonInput(buttonInput);
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(_upControl.Button))
        {
            DoInput(_upControl.InputAssignment);
        }
        if (Input.GetKeyDown(_leftControl.Button))
        {
            DoInput(_leftControl.InputAssignment);
        }
        if (Input.GetKeyDown(_rightControl.Button))
        {
            DoInput(_rightControl.InputAssignment);
        }
        if (Input.GetKeyDown(_downControl.Button))
        {
            DoInput(_downControl.InputAssignment);
        }
    }
}

[System.Serializable]
public struct Control
{
    [SerializeField] private ButtonInput _inputAssignment;
    public ButtonInput InputAssignment => _inputAssignment;

    [SerializeField] private KeyCode _button;
    public KeyCode Button => _button;

    [SerializeField] private ColorObject_new _colorObject;
    public ColorObject_new ColorObjectNew => _colorObject;
}
