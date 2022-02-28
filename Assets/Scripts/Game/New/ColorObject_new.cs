using UnityEngine;

public class ColorObject_new : MonoBehaviour
{
    private ColorLight _colorLight;
    public ColorLight ClrLight => _colorLight;

    public ColorOption CurrentColor;
    public ButtonInput ButtonAssignment { get; set; }

    private Tweener_Simple _tweenerSimple;

    private void Awake()
    {
        _colorLight = GetComponentInChildren<ColorLight>();
        _tweenerSimple = GetComponent<Tweener_Simple>();
    }

    private void OnEnable()
    {
        EventManager.OnButtonInput += Pressed;
    }

    private void OnDisable()
    {
        EventManager.OnButtonInput -= Pressed;
    }

    public void Pressed(ButtonInput buttonInput)
    {
        if (buttonInput != ButtonAssignment)
            return;

        float flashDuration = 0.2f;

        if (_tweenerSimple)
        {
            _tweenerSimple.PlayTween();
            flashDuration = 2 * _tweenerSimple.TweenData.Duration;
        }

        if (_colorLight)
        {
            _colorLight.LightFlashTime = flashDuration;
            _colorLight.FlashLight();
        }
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
