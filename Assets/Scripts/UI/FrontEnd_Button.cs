using UnityEngine;
using DG.Tweening;

public class FrontEnd_Button : MonoBehaviour
{
    RectTransform startTransform;
    Vector3 startScale;

    Tween hoverTween;
    Tween selectTween;

    [Header("Hover Tween")]
    [SerializeField] Ease pulseEaseType;
    [SerializeField] Vector3 pulseScale;
    [SerializeField][Range(0,5)] float pulseDuration;

    [Header("Select Tween")]
    [SerializeField] Ease punchEaseType;
    [SerializeField] Vector3 punchScale;
    [SerializeField][Range(0,5)] float punchDuration;
    [SerializeField] int punchVibrato;
    [SerializeField] float punchElasticity;

    private void Awake()
    {
        startTransform = GetComponent<RectTransform>();
        startScale = startTransform.localScale;
    }

    void OnEnable()
    {
        CreateTweens();
    }

    void OnDisable()
    {
        KillTweens();
        transform.localScale = startScale;
    }

    private void KillTweens()
    {
        transform.DOKill();
    }

    void CreateTweens()
    {
        hoverTween = transform.DOScale(pulseScale, pulseDuration).SetEase(pulseEaseType).SetLoops(-1, LoopType.Yoyo).SetAutoKill(false);
        hoverTween.Pause();

        selectTween = transform.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElasticity).SetEase(punchEaseType).SetAutoKill(false);
        selectTween.Pause();
    }

    public void PlayHoverAnimation()
    {
        hoverTween.Restart();
    }

    public void StopHoverAnimation()
    {
        hoverTween.Rewind();
    }

    public void PlaySelectAnimation()
    {
        selectTween.Restart();
    }

}
