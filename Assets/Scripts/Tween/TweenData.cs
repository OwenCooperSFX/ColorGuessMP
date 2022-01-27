using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class TweenData
{
    public enum TweenType { Scale, Move, Rotate }

    [Header("Basic Settings")]
    public TweenType tweenType;
    [Tooltip("Object to tween.")]
    public GameObject targetObject;
    [Tooltip("Set a starting value different from the current one.")]
    public Vector3 startOffset;
    [Tooltip("Position, scale or rotation at the end of the tween relative to its parameter before starting.")]
    public Vector3 endOffset;
    [Tooltip("How long (in seconds) for the tween to complete.")]
    public float duration = 1f;
    [Tooltip("How long to wait (in seconds) before playing tween.")]
    public float delay = 0f;

    [Header("Easing")]
    public Ease ease = Ease.Linear;

    [Header("Looping")]
    [Tooltip("Set to -1 for infinite looping")]
    public int loops = 0;
    public LoopType loopType = LoopType.Restart;

    [Header("Sequencing")]
    [Tooltip("If true, play tween concurrently in its sequence.")]
    public bool join = false;

    public Tween GetTween()
    {
        Tween tween = null;

        switch (tweenType)
        {
            case (TweenType.Move):
                tween = CreateMoveTween();
                break;
            case (TweenType.Scale):
                tween = CreateScaleTween();
                break;
            case (TweenType.Rotate):
                tween = CreateRotateTween();
                break;
        }

        return tween;
    }

    Tween CreateMoveTween()
    {
        if (!targetObject)
        {
            Debug.LogError("No target object specified!");
            return null;
        }

        Vector3 targetPosition = targetObject.transform.localPosition + endOffset;
        targetObject.transform.localPosition += startOffset;
        Tween _tween = targetObject.transform.DOLocalMove(targetPosition, duration).SetEase(ease).SetLoops(loops, loopType).SetDelay(delay);

        return _tween;
    }

    Tween CreateScaleTween()
    {
        if (!targetObject)
        {
            Debug.LogError("No target object specified!");
            return null;
        }

        Vector3 targetScale = targetObject.transform.localScale + endOffset;
        targetObject.transform.localScale += startOffset;
        Tween _tween = targetObject.transform.DOScale(targetScale, duration).SetEase(ease).SetLoops(loops, loopType).SetDelay(delay);

        return _tween;
    }

    Tween CreateRotateTween()
    {
        if (!targetObject)
        {
            Debug.LogError("No target object specified!");
            return null;
        }

        Vector3 targetRotation = targetObject.transform.localEulerAngles + endOffset;
        targetObject.transform.localEulerAngles += startOffset;
        Tween _tween = targetObject.transform.DOLocalRotate(targetRotation, duration).SetEase(ease).SetLoops(loops, loopType).SetDelay(delay);

        return _tween;
    }
}
