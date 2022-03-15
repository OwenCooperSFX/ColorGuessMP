using System.Collections.Generic;
using UnityEngine;

public enum RepetitionOption { TrueRandom, ReduceRepeat, NoRepeat }

public class ColorObjectPrompt : ColorObjectBase
{
    [SerializeField] RepetitionOption _repetitionOption = RepetitionOption.NoRepeat;

    [SerializeField] private List<Color> _colors = new List<Color>();

    private int _length = 0;
    private ColorOption _lastColor;

    public void OnEnable()
    {
        EventManager.OnColorObjectButtonSelected += HandleColorObjectButtonSelected;
    }

    public void OnDisable()
    {
        EventManager.OnColorObjectButtonSelected -= HandleColorObjectButtonSelected;
    }

    private void Awake()
    {
        InitializeColors();
    }

    /// <summary>
    /// DEBUGGING ONLY. REMOVE LATER
    /// </summary>
    /// 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AssignRandomColor();
    }

    private void InitializeColors()
    {
        _colors.Add(Color.red);
        _colors.Add(Color.green);
        _colors.Add(Color.blue);
        _colors.Add(Color.yellow);

        _length = _colors.Count;
    }

    private void AssignRandomColor()
    {
        _lastColor = GetColorOptionFromMaterial();

        Color rndColor = _colors[Random.Range(0, _length)];
        UpdateCurrentColor(rndColor);

        switch (_repetitionOption)
        {
            case RepetitionOption.TrueRandom:           /*already shuffled*/ ; break;
            case RepetitionOption.ReduceRepeat:         RandomizeReduceRepeat(rndColor); break;
            case RepetitionOption.NoRepeat:             RandomizeNoRepeat(rndColor); break;
            default:                                    RandomizeNoRepeat(rndColor); break;
        }
    }

    private void HandleColorObjectButtonSelected(ColorObjectBase callingColorObject)
    {
        string message = callingColorObject.ToString();

        ColorOption promptColor = GetColorOptionFromMaterial();

        if (callingColorObject.CurrentColor == promptColor)
        {
            EventManager.RaiseColorMatch(callingColorObject.OwningPlayer);
            message += " SUCCESS!";
            AssignRandomColor();
        }
        else
        {
            EventManager.RaiseColorMismatch(callingColorObject.OwningPlayer);
            message += " FAIL!";
        }

        print(message);
    }

    private void RandomizeReduceRepeat(Color newColor)
    {
        if (CurrentColor == _lastColor)
        {
            newColor = _colors[Random.Range(0, _length)];
            UpdateCurrentColor(newColor);
            print(this + ": Re-randomize!");
        }
    }

    private void RandomizeNoRepeat(Color newColor)
    {
        while (CurrentColor == _lastColor)
        {
            newColor = _colors[Random.Range(0, _length)];
            UpdateCurrentColor(newColor);
            print(this + ": Re-randomize!");

            if (CurrentColor != _lastColor)
                break;
        }
    }
}
