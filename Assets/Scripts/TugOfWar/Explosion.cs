using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem particleSystem;

    bool enable;

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

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        enable = particleSystem.IsAlive();

        if (!enable)
        {
            OnExplosionFinished();
            OnDisable();
        }
    }

    private void OnEnable()
    {
        RaiseExplosionStarted();
    }

    private void OnDisable()
    {
        
    }
}
