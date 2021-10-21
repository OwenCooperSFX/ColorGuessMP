using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem particleSystem;

    bool enable;

    public float shakeLength, shakePower;

    [SerializeField] Light _light;
    public Light light { get { return _light; } set { _light = light; } }

    private float startIntensity;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

        _light = transform.GetChild(0).GetComponent<Light>();
        startIntensity = _light.intensity;
    }

    void Start()
    {

    }

    private void Update()
    {
        enable = particleSystem.IsAlive();

        if (enable)
        {
            light.intensity -= 100 * Time.deltaTime;
        }

        if (!enable)
        {
            EventManager.RaiseExplosionFinished();
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Explode();
    }

    void Explode()
    {
        light.intensity = startIntensity;

        ScreenshakeController.Instance.StartShake(shakeLength, shakePower);

        EventManager.RaiseExplosionStarted();
    }
}
