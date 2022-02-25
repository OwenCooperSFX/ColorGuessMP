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
        if(_colorLight)
            _colorLight.FlashLight();
        if(_tweenerSimple)
            _tweenerSimple.PlayTween();
    }
}
