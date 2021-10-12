using UnityEngine;
using DG.Tweening;

public class BadThing : MonoBehaviour
{
    Tween dangerPulseTween;
    Tween sizeIncreaseTween;
    Tween sizeDecreaseTween;

    Vector3 startScale = Vector3.one;
    //public Vector3 growAmount = new Vector3(1,1,1);

    public float defaultGrowSpeed = 1;

    private void Update()
    {
        //TODO:Just determine scaling with update -- relative to center vs horizontal limit. 
    }

    private void Awake()
    {
        //CreateTweens();
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
