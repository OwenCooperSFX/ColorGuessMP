using DG.Tweening;
using UnityEngine;
using System.Collections;

public enum TweenAnimType { Scale, Move, Rotate }
public class Tweener_Simple : MonoBehaviour
{
    [Tooltip("Defaults to owning GameObject if left empty.")]
    public GameObject targetObject;

    [SerializeField]
    private TweenAnimType _tweenAnimType;
    public TweenAnimType tweenAnimType { get { return _tweenAnimType; } }

    [Tooltip("Scale: start transform scale, Move: start transform position, Rotate: start transform rotation.")]
    public Vector3 startOffset;
    [Tooltip("Scale: end transform scale, Move: end transform position, Rotate: end transform rotation.")]
    public Vector3 destination;
    [Tooltip("Time in seconds for tween to complete one loop.")]
    public float duration = 1f;

    public Ease ease = Ease.Linear;

    public LoopType loopType = LoopType.Restart;
    [Tooltip("Set to -1 for infinite looping")]
    public int loops = 0;

    public float startDelay = 0f;

    public bool playOnEnable = true;
    public bool resetOnDisable =  false;

    public Tween tween { get; private set; }
    public Transform startTransform { get; private set; }
    private Vector3 startScale;
    private Vector3 startPosition;
    private Vector3 startRotation;

    private void Awake()
    {
        if (!targetObject)
            targetObject = gameObject;

        startTransform = targetObject.GetComponent<Transform>();

        startScale = startTransform.localScale;
        startPosition = startTransform.localPosition;
        startRotation = startTransform.localRotation.eulerAngles;

        //if (from == Vector3.zero)
        //{
        //    switch (_tweenAnimType)
        //    {
        //        case TweenAnimType.Scale:
        //            from = startScale;
        //            break;
        //        case TweenAnimType.Move:
        //            from = startPosition;
        //            break;
        //        case TweenAnimType.Rotate:
        //            from = startRotation;
        //            break;
        //    }
        //}
    }

    private void OnEnable()
    {
        SetDOTweenType(_tweenAnimType);

        if (playOnEnable)
            PlayTween();
    }

    private void OnDisable()
    {
        tween.Kill();

        if (resetOnDisable)
            ResetTransform();
    }

    void CreateScaleTween()
    {
        tween = targetObject.transform.DOScale(destination, duration).SetEase(ease).SetLoops(loops, loopType);
        tween.Pause();
    }

    void CreateMoveTween()
    {
        tween = targetObject.transform.DOLocalMove(destination, duration).SetEase(ease).SetLoops(loops, loopType);
        tween.Pause();
    }

    void CreateRotateTween()
    {
        tween = targetObject.transform.DOLocalRotate(destination, duration).SetEase(ease).SetLoops(loops, loopType);
        tween.Pause();
    }

    public void SetDOTweenType(TweenAnimType tweenAnim)
    {
        switch (tweenAnim)
        {
            case TweenAnimType.Scale:
                CreateScaleTween();
                break;
            case TweenAnimType.Move:
                CreateMoveTween();
                break;
            case TweenAnimType.Rotate:
                CreateRotateTween();
                break;
        }

        _tweenAnimType = tweenAnim;
    }

    public void PlayTween()
    {
        StartCoroutine(PlayTweenWithDelay(startDelay));
    }

    public void PauseTween()
    {
        if (tween.IsPlaying())
            tween.Pause();
    }

    IEnumerator PlayTweenWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!tween.IsPlaying())
            tween.Play();
    }

    void ResetTransform()
    {
        switch (_tweenAnimType)
        {
            case TweenAnimType.Scale:
                targetObject.transform.localScale = startScale;
                break;
            case TweenAnimType.Move:
                targetObject.transform.localPosition = startPosition;
                break;
            case TweenAnimType.Rotate:
                targetObject.transform.localRotation = Quaternion.Euler(startRotation);
                break;
        }
    }

    protected void SetStartOffset(Vector3 _offset)
    {
        _offset += startOffset;

        switch (_tweenAnimType)
        {
            case TweenAnimType.Scale:
                targetObject.transform.localScale = startOffset;
                break;
            case TweenAnimType.Move:
                targetObject.transform.localPosition = startOffset;
                break;
            case TweenAnimType.Rotate:
                targetObject.transform.localRotation = Quaternion.Euler(startOffset);
                break;
        }
    }
}
