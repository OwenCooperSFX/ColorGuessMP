using UnityEngine;

public class ColorObject_new : MonoBehaviour
{
    private ColorLight _colorLight;
    public ColorLight ColourLight => _colorLight;

    public ColorOption CurrentColor;

    private Tweener_Simple _tweenerSimple;

    private void Awake()
    {
        _colorLight = GetComponentInChildren<ColorLight>();
        _tweenerSimple = GetComponent<Tweener_Simple>();
    }

    public void Pressed()
    {
        float flashDuration = 0.2f;

        if (_tweenerSimple)
        {
            _tweenerSimple.PlayTween();
            flashDuration = 2 * _tweenerSimple.TweenDataSoRef.Duration;
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

        CurrentColor = GetColorOptionFromMaterialColor();
    }

    public ColorOption GetColorOptionFromMaterialColor()
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
