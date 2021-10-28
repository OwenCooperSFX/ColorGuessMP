using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem particleSystem;

    bool enable;

    public float shakeLength, shakePower;

    [SerializeField] Light _explosionLight;
    public Light explosionLight { get { return _explosionLight; } set { _explosionLight = explosionLight; } }

    private float startIntensity;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

        _explosionLight = transform.GetChild(0).GetComponent<Light>();
        startIntensity = _explosionLight.intensity;
    }

    private void Update()
    {
        enable = particleSystem.IsAlive();

        if (enable)
        {
            _explosionLight.intensity -= 100 * Time.deltaTime;
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
        _explosionLight.intensity = startIntensity;

        ScreenshakeController.Instance.StartShake(shakeLength, shakePower);

        EventManager.RaiseExplosionStarted();
    }
}
