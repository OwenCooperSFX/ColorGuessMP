using UnityEngine;
using DG.Tweening;

public class FrontEnd_Button : MonoBehaviour
{
    RectTransform startTransform;
    Vector3 startScale;

    Tween hoverTween;
    Tween selectTween;

    [Header("Hover Tween")]
    [SerializeField] Ease pulseEaseType = Ease.InOutSine;
    [SerializeField] Vector3 pulseScale = new Vector3(1.05f, 1.1f ,0f);
    [SerializeField][Range(0,5)] float pulseDuration = 0.5f;

    [Header("Select Tween")]
    [SerializeField] Ease punchEaseType = Ease.InExpo;
    [SerializeField] Vector3 punchScale = new Vector3(-0.2f, -0.2f, 0f);
    [SerializeField][Range(0,5)] float punchDuration = 0.3f;
    [SerializeField] int punchVibrato = 8;
    [SerializeField] float punchElasticity = 10f;

    private void Awake()
    {
        startTransform = GetComponent<RectTransform>();
        startScale = startTransform.localScale;
    }

    private void OnEnable()
    {
        CreateTweens();
    }

    private void OnDisable()
    {
        KillTweens();
        transform.localScale = startScale;
    }

    private void KillTweens()
    {
        transform.DOKill();
    }

    private void CreateTweens()
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
