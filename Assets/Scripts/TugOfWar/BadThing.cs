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

        // TODO: Optimize and smooth out. Get away from update if possible. SHould be able to scale with Tweening.
        scaleMagnitude = Mathf.Pow(1 + (Mathf.Abs(transform.position.x)/5f), 2);
        currentScale = new Vector3(scaleMagnitude, scaleMagnitude, scaleMagnitude);
        transform.localScale = currentScale;
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
}
