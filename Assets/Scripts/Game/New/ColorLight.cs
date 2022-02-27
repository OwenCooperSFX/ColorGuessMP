using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class ColorLight : MonoBehaviour
{
    [SerializeField] private ColorLightDataSO _colorLightDataSO;

    private Light _light;
    private float _maxIntensity;
    private float _baseIntensity;

    private ColorLightDataSO.IntensityMultipliers _multipliers;

    private ColorObject_new _parentColorObjectNew;
    private ColorOption _currentColor;
    private Coroutine _flashLightCR;

    public float LightFlashTime { get; set; }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _light = GetComponent<Light>();
        _light.intensity = 0;

        if (_colorLightDataSO)
            UpdateColorLightData();

        _maxIntensity = _baseIntensity;

        if (transform.parent)
            _parentColorObjectNew = transform.GetComponentInParent<ColorObject_new>();
        else
            Debug.LogError(this + " requires a parent GameObject of type " + _parentColorObjectNew.GetType() + ".");
    }

    private void UpdateColorLightData()
    {
        {
            _light.range = _colorLightDataSO.LightRange;

            LightFlashTime = _colorLightDataSO.LightFlashTime;
            _baseIntensity = _colorLightDataSO.BaseIntensity;
            _multipliers = _colorLightDataSO.Multipliers;
        }
    }

    public void FlashLight()
    {
        if (_light)
        {
            UpdateColorLightData();
            UpdateColor();

            _flashLightCR = StartCoroutine(FlashLightCR());
        }
    }

    private void UpdateColor()
    {
        if (_parentColorObjectNew)
            _currentColor = _parentColorObjectNew.CurrentColor;

        switch (_currentColor)
        {
            case ColorOption.blue:
                _light.color = Color.blue;
                _maxIntensity = (_multipliers.Blue * _baseIntensity);
                break;
            case ColorOption.green:
                _light.color = Color.green;
                _maxIntensity = (_multipliers.Green * _baseIntensity);
                break;
            case ColorOption.red:
                _light.color = Color.red;
                _maxIntensity = (_multipliers.Red * _baseIntensity);
                break;
            case ColorOption.yellow:
                _light.color = Color.yellow;
                _maxIntensity = (_multipliers.Yellow * _baseIntensity);
                break;
            case ColorOption.invalid:
                Debug.LogWarning("Invalid color!");
                break;
        }
    }

    private IEnumerator FlashLightCR()
    {
        if (_flashLightCR != null)
            StopCoroutine(_flashLightCR);

        _light.intensity = _maxIntensity;

        while (_light.intensity > 0)
        {
            _light.intensity -= (_maxIntensity / LightFlashTime) * Time.deltaTime;
            yield return null;
        }

        _light.intensity = 0;
        yield break;
    }
}
