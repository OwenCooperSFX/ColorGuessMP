using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem particleSystem;

    bool enable;

    public float shakeLength, shakePower;

    public delegate void ExplosionEvent();
    public event ExplosionEvent OnExplosionStarted;
    public event ExplosionEvent OnExplosionFinished;

    void RaiseExplosionStarted()
    {
        if (OnExplosionStarted != null)
            OnExplosionStarted();
    }

    void RaiseExplosionFinished()
    {
        if (OnExplosionFinished != null)
            OnExplosionFinished();
    }

    private void Awake()
    {
    }

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        enable = particleSystem.IsAlive();

        if (!enable)
        {
            RaiseExplosionFinished();
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Explode();
    }

    void Explode()
    {
        ScreenshakeController.Instance.StartShake(shakeLength, shakePower);

        RaiseExplosionStarted();
    }
}
