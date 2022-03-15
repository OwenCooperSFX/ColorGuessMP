using System.Collections.Generic;
using UnityEngine;

public class ControlColorHandler : MonoBehaviour
{
    [SerializeField] private List<Color> _colors = new List<Color>();
    private List<Color> _lastColorOrder = new List<Color>();

    public List<Color> ColorAssignments { get; private set; } = new List<Color>();

    private void Awake()
    {
        InitializeColors();
    }

    private void InitializeColors()
    {
        _colors.Add(Color.red);
        _colors.Add(Color.green);
        _colors.Add(Color.blue);
        _colors.Add(Color.yellow);

        _lastColorOrder = _colors;
        ColorAssignments = _colors;

        AssignControlColors();
    }

    public List<Color> ShuffleColors()
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

    public void AssignControlColors()
    {
        if (ColorAssignments == _lastColorOrder)
        {
            ColorAssignments = ShuffleColors();
            _lastColorOrder = ColorAssignments;
        }
    }

    public void RenderMaterialColors(List<Control> controls)
    {
        if (controls.Count <= 0)
            return;

        for (int i = 0; i < controls.Count; i++)
        {
            ColorObjectBase colorObject = controls[i].ColorObjectBase;

            colorObject.UpdateCurrentColor(ColorAssignments[i]);
        }
    }
}
