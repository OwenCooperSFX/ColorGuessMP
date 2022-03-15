using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerControls")]
public class PlayerControlsSO : ScriptableObject
{
    [SerializeField] private Control _upControl, _leftControl, _downControl, _rightControl;

    public Control UpControl => _upControl;
    public Control LeftControl => _leftControl;
    public Control DownControl => _downControl;
    public Control RightControl => _rightControl;

    public List<Control> Controls { get; } = new List<Control>();

    private void OnEnable()
    {
        Controls.Add(_upControl);
        Controls.Add(_leftControl);
        Controls.Add(_downControl);
        Controls.Add(_rightControl);
    }

    private void OnDisable()
    {
        Controls.Clear();
    }
}

//[System.Serializable]
//public struct Control
//{
//    [SerializeField] private ButtonInput _inputAssignment;
//    public ButtonInput InputAssignment => _inputAssignment;

//    [SerializeField] private KeyCode _button;
//    public KeyCode Button => _button;
//}
