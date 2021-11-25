using UnityEngine;
using DG.Tweening;

public class BadThing : MonoBehaviour
{
    Tween dangerPulseTween;
    Tween sizeIncreaseTween;
    Tween sizeDecreaseTween;

    Vector3 startScale = Vector3.one;
    //public Vector3 growAmount = new Vector3(1,1,1);

    float scaleMagnitude =  0f;
    float maxScaleMagnitude = 5f;
    Vector3 currentScale = Vector3.one;

    public float defaultGrowSpeed = 1;

    private TugOfWar tugOfWarRef;

    private void OnEnable()
    {
        EventManager.OnBadThingMoved += Pulse;
    }
    private void OnDisable()
    {
        EventManager.OnBadThingMoved -= Pulse;
    }

    private void Awake()
    {
        //CreateTweens();
        tugOfWarRef = FindObjectOfType<TugOfWar>();
        scaleMagnitude = startScale.magnitude;
        maxScaleMagnitude *= 1 + (tugOfWarRef.horizontalLimit/10f);
        currentScale = transform.localScale;
    }

    private void Update()
    {
        //TODO:Just determine scaling with update -- relative to center vs horizontal limit. 

        // TODO: Optimize and smooth out. Get away from update if possible. Should be able to scale with Tweening.
        scaleMagnitude = Mathf.Pow(1 + (Mathf.Abs(transform.position.x)/10f), 2);
        currentScale = new Vector3(scaleMagnitude, scaleMagnitude, scaleMagnitude);
        transform.localScale = currentScale;

        // This can work, but it's too much.
        transform.parent.localPosition = Shake(Mathf.Abs(transform.position.x));

        // Doesnt work :(
        //UpdateDangerTween(Mathf.Abs(transform.position.x));
    }

    void CreateTweens()
    {
        //sizeIncreaseTween = transform.DOScale(transform.localScale + growAmount, growSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutElastic).SetAutoKill(false);
        //sizeIncreaseTween.Pause();

        //sizeDecreaseTween = transform.DOScale(transform.localScale - growAmount, growSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutElastic).SetAutoKill(false);
        //sizeDecreaseTween.Pause();
    }

    public void IncreaseSize(Vector3 growAmount, float growSpeed)
    {
        sizeIncreaseTween = transform.DOScale(transform.localScale + growAmount, growSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBounce).SetAutoKill(false);
        // sizeIncreaseTween.Play();
    }

    public void DecreaseSize(Vector3 shrinkAmount, float shrinkSpeed)
    {
        sizeDecreaseTween = transform.DOScale(transform.localScale - shrinkAmount, shrinkSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBounce).SetAutoKill(false);
        // sizeDecreaseTween.Play();
    }

    public void PauseTweens()
    {
        sizeIncreaseTween.Pause();
        sizeDecreaseTween.Pause();
    }

    public void KillTweens()
    {
        sizeIncreaseTween.Kill();
        sizeDecreaseTween.Kill();
    }


    // Doesn't work :(
    public void UpdateDangerTween(float in_DangerAmount)
    {
        float vibration = Mathf.Clamp(Random.Range(0f, 1f) * in_DangerAmount, 0f, 2f);

        Vector3 newPosition = new Vector3(vibration, vibration, 0f);
        dangerPulseTween = transform.DOLocalMove(newPosition, 1 / in_DangerAmount);
    }


    // This can work, but we need a holder to maintain the x-position so it's unaffected by the shaking.
    Vector3 Shake(float in_DangerAmount)
    {
        in_DangerAmount *= 0.01f;

        float vibrationX = Mathf.Clamp(Random.Range(-1f, 1f) * in_DangerAmount, -2f, 2f);
        float vibrationY = Mathf.Clamp(Random.Range(-1f, 1f) * in_DangerAmount, -2f, 2f);

        Vector3 newPosition = new Vector3(vibrationX, vibrationY, 0f);

        return newPosition;
    }

    void Pulse()
    {
        //// TODO: use events to simplify this.
        //if (Mathf.Abs(TugOfWar.Instance.badThing_xPos) >= TugOfWar.Instance.horizontalLimit)
        //{
        //    // Go back to default speed if boundary was exceeded.
        //    transform.parent.DOScale(transform.localScale * 1.1f, .3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        //}
        //else
        //{
        //    // Scale Tween speed based on BadThing position if boundary not exceeded.
        //    transform.parent.DOScale(transform.localScale * 1.1f, (Mathf.Clamp(.5f / Mathf.Abs(TugOfWar.Instance.badThing_xPos), .01f, .3f)))
        //        .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        //}
    }
}
