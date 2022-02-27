using UnityEngine;

public class ColorObject_new : MonoBehaviour
{
    private ColorLight _colorLight;
    public ColorLight ColorLight => _colorLight;

    public ColorOption CurrentColor;

    private Tweener_Simple _tweenerSimple;
    public Tweener_Simple TweenerSimple => _tweenerSimple;

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
}
