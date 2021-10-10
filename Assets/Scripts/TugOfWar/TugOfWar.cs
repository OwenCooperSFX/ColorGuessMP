using UnityEngine;

public class TugOfWar : MonoBehaviour
{
    [SerializeField] GameObject _badThing;
    public GameObject badThing { get { return _badThing; } }

    [SerializeField] float _pushAmount;
    public float pushAmount { get { return _pushAmount; } }

    [SerializeField] GameObject _track;
    public GameObject track { get { return _track; } }

    [SerializeField] AudioClip _explosionSound;
    public AudioClip explosionSound { get { return _explosionSound; } }

    float horizontalLimit;

    MeshRenderer meshRenderer;

    Vector3 startPosition;

    GameObject explosion;

    AudioSource badThingAudioSource;

    bool bEmitting;

    public delegate void ExplosionFinished();
    public event ExplosionFinished OnExplosionFinished;

    void RaiseExplosionFinished()
    {
        if (OnExplosionFinished != null)
        {
            OnExplosionFinished();
        }
    }

    void Awake()
    {
        explosion = badThing.transform.GetChild(0).gameObject;
        meshRenderer = badThing.GetComponent<MeshRenderer>();
        startPosition = badThing.transform.position;
        horizontalLimit = track.transform.localScale.y;
        badThingAudioSource = badThing.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (explosion.GetComponent<ParticleSystem>().IsAlive())
        {
            bEmitting = true;
        }
        else if (bEmitting)
        {
            RaiseExplosionFinished();
            bEmitting = false;

            ResetBadThing();
        }

        if (!Input.anyKey)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveBadThing(pushAmount);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            MoveBadThing(-pushAmount);
        }
    }

    void MoveBadThing(float amount)
    {
        if (badThing.transform.position.x > -horizontalLimit && badThing.transform.position.x < horizontalLimit)
        {
            Vector3 translation = new Vector3(amount, 0, 0);

            badThing.transform.Translate(translation);
        }
        else
        {
            ShowPlayerWon();
        }
    }

    void ShowPlayerWon()
    { 
        if (meshRenderer.enabled)
        {
            badThingAudioSource.PlayOneShot(explosionSound);
        }

        meshRenderer.enabled = false;
        explosion.SetActive(true);
    }

    void ResetBadThing()
    {
        badThing.transform.position = startPosition;
        meshRenderer.enabled = true;
        explosion.SetActive(false);
    }

    void InterpTranslate(Vector3 targetPosition)
    {
        //Mathf.
    }
}
