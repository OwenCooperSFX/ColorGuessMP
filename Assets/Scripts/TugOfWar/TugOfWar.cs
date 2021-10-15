using UnityEngine;
using DG.Tweening;

public class TugOfWar : MonoBehaviour
{
    public static TugOfWar Instance;

    [SerializeField] GameObject _badThing;
    public GameObject badThing { get { return _badThing; } }

    [SerializeField] float _pushAmount;
    public float pushAmount { get { return _pushAmount; } }

    [SerializeField] float _pushDuration;
    public float pushSpeed { get { return _pushDuration; } }

    [SerializeField] GameObject _track;
    public GameObject track { get { return _track; } }

    [SerializeField] AudioClip[] _explosionSounds;
    public AudioClip[] explosionSounds { get { return _explosionSounds; } }

    float _horizontalLimit;
    public float horizontalLimit { get { return _horizontalLimit; } }

    public float badThing_xPos;
    MeshRenderer meshRenderer;
    Vector3 startPos;
    float last_xPos;
    AudioSource badThingAudioSource;

    Explosion _explosion;
    public Explosion explosion { get { return _explosion; } }

    Tween pushTween;
    float pulseSpeed = 1;

    bool bEmitting;

    #region Event dispatchers

    public delegate void TugOfWarEvent();
    public event TugOfWarEvent OnExplosionFinished;
    public event TugOfWarEvent OnExceededBoundary;
    public event TugOfWarEvent OnBadThingMoved;

    void RaiseExplosionFinished()
    {
        if (OnExplosionFinished != null)
        {
            OnExplosionFinished();
        }
    }

    void RaiseExceededBoundary()
    {
        if (OnExceededBoundary != null)
        {
            OnExceededBoundary();
        }
    }

    void RaiseBadThingMoved()
    {
        if (OnBadThingMoved != null)
        {
            OnBadThingMoved();
        }
    }
    #endregion

    #region Event subscribers
    void OnEnable()
    {
        OnBadThingMoved += CheckBoundary;
        OnExceededBoundary += ShowPlayerWon;

        if (_explosion)
            _explosion.OnExplosionFinished += ResetBadThing;
    }

    void OnDisable()
    {
        OnBadThingMoved += CheckBoundary;
        OnExceededBoundary -= ShowPlayerWon;

        if (_explosion)
            _explosion.OnExplosionFinished -= ResetBadThing;
    }
    #endregion

    void Awake()
    {
        Init();

        _explosion = badThing.transform.GetChild(0).GetComponent<Explosion>();
        meshRenderer = badThing.GetComponent<MeshRenderer>();
        startPos = badThing.transform.position;
        _horizontalLimit = track.transform.localScale.y;
        badThingAudioSource = badThing.GetComponent<AudioSource>();
    }

    void Init()
    {
        // Singleton logic

        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        #region Input
        /*
        if (!Input.anyKey)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveBadThing(pushAmount, 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            MoveBadThing(-pushAmount, 0.5f);
        }
        */
        #endregion
    }

    public void MoveBadThing(float amount, float speed)
    {
        float new_xPos = badThing_xPos + amount;

        if (badThing.transform.position.x > -_horizontalLimit && badThing.transform.position.x < _horizontalLimit)
        {
            pushTween = badThing.transform.DOLocalMoveX(new_xPos, speed).SetEase(Ease.OutExpo);
        }

        badThing_xPos = new_xPos;

        RaiseBadThingMoved();
    }

    void ShowPlayerWon()
    { 
        if (meshRenderer.enabled)
        {
            foreach (AudioClip clip in explosionSounds)
            {
                badThingAudioSource.PlayOneShot(clip);
            }
        }

        meshRenderer.enabled = false;
        _explosion.gameObject.SetActive(true);
    }

    void ResetBadThing()
    {
        badThing.transform.position = startPos;
        meshRenderer.enabled = true;
        _explosion.gameObject.SetActive(false);

        badThing_xPos = badThing.transform.localPosition.x;
        badThing.GetComponent<BadThing>().KillTweens();
        badThing.transform.localScale = Vector3.one;
    }

    void CheckBoundary()
    {
        //Animations. TODO: just handle in Update on BadThing
        if (badThing_xPos >= -_horizontalLimit && badThing_xPos < 0)
        {
            //float scaleFactor = 2 / Mathf.Abs(-horizontalLimit - badThing_xPos);
            //Vector3 scale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            //if (badThing_xPos < last_xPos)
            //    badThing.GetComponent<BadThing>().IncreaseSize(scale, Mathf.Clamp((pulseSpeed *= .5f), .1f, 6f));
            //else if (badThing_xPos > last_xPos)
            //    badThing.GetComponent<BadThing>().DecreaseSize(scale, Mathf.Clamp((pulseSpeed *= 2f), .1f, 6f));

            last_xPos = badThing_xPos;
            return;
        }
        else if (badThing_xPos <= _horizontalLimit && badThing_xPos > 0)
        {
            //float scaleFactor = 2 / Mathf.Abs(horizontalLimit - badThing_xPos);
            //Vector3 scale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            //if (badThing_xPos > last_xPos)
            //    badThing.GetComponent<BadThing>().IncreaseSize(scale, Mathf.Clamp((pulseSpeed *= .5f), .1f, 6f));
            //else if (badThing_xPos < last_xPos)
            //    badThing.GetComponent<BadThing>().DecreaseSize(scale, Mathf.Clamp((pulseSpeed *= 2f), .1f, 6f));

            last_xPos = badThing_xPos;
            return;
        }
        else
        {
            // If moving will exceed a boundary, set position instead to the boundary.
            RaiseExceededBoundary();

            if (badThing_xPos < -_horizontalLimit)
            {
                _badThing.transform.localPosition = new Vector3(-_horizontalLimit, 0f, 0f);
            }
            else if (badThing_xPos > _horizontalLimit)
            {
                _badThing.transform.localPosition = new Vector3(_horizontalLimit, 0f, 0f);
            }
        }

        pushTween.TogglePause();
        //badThing.transform.localPosition = new Vector3(badThing_xPos, badThing.transform.position.y, badThing.transform.position.z);
    }
}
