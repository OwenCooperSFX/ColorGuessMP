using UnityEngine;

[CreateAssetMenu(menuName = "ColorLightData")]
public class ColorLightDataSO : ScriptableObject
{
    [Space(10)]
    [SerializeField] [Range(0.1f, 20f)] private float _baseIntensity = 5.0f;
    [SerializeField] [Range(0.1f, 20f)] private float _range = 1.8f;

    [Space(10)]
    [SerializeField] private IntensityMultipliers _intensityMultiplers;

    //This is being set via the ColorLight's parent ColorObject. Otherwise is just a default non-zero value. Can serialize this if we want to decouple.
    private float _lightFlashTime = 0.2f;

    public float BaseIntensity => _baseIntensity;
    public float LightRange => _range;
    public float LightFlashTime { get => _lightFlashTime; set => _lightFlashTime = value; }
    public IntensityMultipliers Multipliers => _intensityMultiplers;


    [System.Serializable]
    public struct IntensityMultipliers
    {
        [SerializeField] [Range(0.1f, 20f)] private float _blue;
        [SerializeField] [Range(0.1f, 20f)] private float _green;
        [SerializeField] [Range(0.1f, 20f)] private float _red;
        [SerializeField] [Range(0.1f, 20f)] private float _yellow;

        public float Blue => _blue;
        public float Green => _green;
        public float Red => _red;
        public float Yellow => _yellow;
    }
}
