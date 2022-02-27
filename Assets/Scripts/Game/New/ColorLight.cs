using System.Collections;
using UnityEngine;

public class ColorLight : MonoBehaviour
{
    private Light _light;
    public ColorOption _currentColor;

    private float _timer = 0;

    [SerializeField] private float _lightFlashTime = 0.2f;
    public float LightFlashTime { get => _lightFlashTime; set => _lightFlashTime = value; }

    private bool _lightOn;

    [SerializeField] private float _defaultMaxIntensity = 5.0f;
    private float _maxIntensity;

    [SerializeField] private float _range = 1.8f;

    Coroutine flashLightCR;

    private void Awake()
    {
        Construct();
    }

    private void DoTimer()
    {
        if (_lightOn)
            _timer += Time.deltaTime;
        else { }
            //EndLight();
    }

    private void Update()
    {
        DoTimer();
    }

    private void Construct()
    {
        _light = GetComponent<Light>();
        _light.intensity = 0;
        _light.range = _range;

        _maxIntensity = _defaultMaxIntensity;

        _timer = _lightFlashTime;
    }

    private void StartLight()
    {
        if (!_light)
            return;

        if (_timer < _lightFlashTime)
        {
            _lightOn = true;

            if (_light.intensity < _maxIntensity)
                _light.intensity += (_maxIntensity / _lightFlashTime) * Time.deltaTime;
            return;
        }
        else
            _lightOn = false;
    }

    private void EndLight()
    {
        if (_light.intensity > 0)
        {
            _light.intensity -= (2 * _maxIntensity / _lightFlashTime) * Time.deltaTime;
        }
    }

    public void FlashLight()
    {
        if (_light)
        {
            _maxIntensity = _defaultMaxIntensity;

            _currentColor = transform.GetComponentInParent<ColorObject_new>().CurrentColor;

            switch (_currentColor)
            {
                case ColorOption.blue:
                    _light.color = Color.blue;
                    _maxIntensity += (.5f * _defaultMaxIntensity);
                    break;
                case ColorOption.green:
                    _light.color = Color.green;
                    _maxIntensity -= (.25f * _defaultMaxIntensity);
                    break;
                case ColorOption.red:
                    _light.color = Color.red;
                    break;
                case ColorOption.yellow:
                    _light.color = Color.yellow;
                    _maxIntensity -= (.25f * _defaultMaxIntensity);
                    break;
                case ColorOption.invalid:
                    Debug.LogWarning("Invalid color!");
                    break;
            }

            flashLightCR = StartCoroutine(FlashLightCR());
        }
    }
    private IEnumerator FlashLightCR()
    {
        if (flashLightCR != null)
            StopCoroutine(flashLightCR);

        _light.intensity = _maxIntensity;

        while (_light.intensity > 0)
        {
            _light.intensity -= (_maxIntensity / _lightFlashTime) * Time.deltaTime;
            yield return null;
        }

        _light.intensity = 0;
        yield break;
    }

    private IEnumerator EndLightCR(float delay = 0)
    {
        while (_light.intensity > 0)
        {
            _light.intensity -= (2 * _maxIntensity / _lightFlashTime) * Time.deltaTime;
        }

        yield return null;
    }
}
