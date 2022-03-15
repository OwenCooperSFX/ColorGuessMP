using UnityEngine;

public abstract class ColorObjectBase : MonoBehaviour
{
    protected ColorLight _colorLight;
    public ColorLight ClrLight => _colorLight;

    public ColorOption CurrentColor;
    public ButtonInput ButtonAssignment { get; set; } = ButtonInput.invalid;

    protected Tweener_Simple _tweenerSimple;

    public PlayerController_new OwningPlayer { get; set; } = null;

    private void Awake()
    {
        _colorLight = GetComponentInChildren<ColorLight>();
        _tweenerSimple = GetComponent<Tweener_Simple>();
    }

    public void UpdateCurrentColor(Color newColor)
    {
        Material material = GetComponent<MeshRenderer>().material;
        material.color = newColor;

        CurrentColor = GetColorOptionFromMaterial();
    }

    public ColorOption GetColorOptionFromMaterial()
    {
        Color color = GetComponent<MeshRenderer>().material.color;
        CurrentColor = ColorOption.invalid;

        if (color == Color.red)
            CurrentColor = ColorOption.red;
        if (color == Color.blue)
            CurrentColor = ColorOption.blue;
        if (color == Color.yellow)
            CurrentColor = ColorOption.yellow;
        if (color == Color.green)
            CurrentColor = ColorOption.green;

        return CurrentColor;
    }
}
