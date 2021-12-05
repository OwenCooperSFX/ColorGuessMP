using UnityEngine;
using DG.Tweening;

public class BadThing : MonoBehaviour
{
    Tween dangerPulseTween;
    Tween sizeIncreaseTween;
    Tween sizeDecreaseTween;

    Vector3 startScale;
    Vector3 parentStartPos;

    float scale =  0f;
    float maxScaleMagnitude = 5f;
    Vector3 currentScale;

    public float defaultGrowSpeed = 1;

    private TugOfWar tugOfWarRef;
    
    bool exploded;

    private void OnEnable()
    {
        EventManager.OnExplosionStarted += UpdateExplodedBool;
        EventManager.OnExplosionFinished += UpdateExplodedBool;
    }

    private void OnDisable()
    {
        EventManager.OnExplosionStarted -= UpdateExplodedBool;
        EventManager.OnExplosionFinished -= UpdateExplodedBool;
    }

    private void Awake()
    {
        //CreateTweens();
        tugOfWarRef = FindObjectOfType<TugOfWar>();
        startScale = transform.localScale;
        scale = startScale.x; // scale will always be uniform so any x/y/z value works
        maxScaleMagnitude *= 1 + (tugOfWarRef.horizontalLimit / 10f);
        currentScale = startScale;

        parentStartPos = transform.parent.localPosition;
        print(currentScale);
    }

    private void Update()
    {
        //TODO:Just determine scaling with update -- relative to center vs horizontal limit. 

        // TODO: Optimize and smooth out. Get away from update if possible. Should be able to scale with Tweening.
        scale = Mathf.Clamp(startScale.x + Mathf.Pow((Mathf.Abs(transform.localPosition.x)/50f), 2), startScale.x, maxScaleMagnitude);
        currentScale = new Vector3(scale, scale, scale);
        transform.localScale = currentScale;

        // This can work, but it's too much.
        if (Mathf.Abs(transform.position.x) > 2.5f && !exploded)
            transform.parent.localPosition = Shake(Mathf.Abs(transform.position.x));

        // Doesnt work :(
        //UpdateDangerTween(Mathf.Abs(transform.localPosition.x));
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
        float vibration = Mathf.Clamp(Random.Range(1f, 2f) * in_DangerAmount, 0f, 2f);

        Vector3 newPosition = new Vector3(vibration, vibration, 0f);
        dangerPulseTween = transform.DOLocalMove(newPosition, .05f);
        dangerPulseTween.Play();
    }


    // This can work, but we need a holder to maintain the x-position so it's unaffected by the shaking.
    Vector3 Shake(float in_DangerAmount)
    {
        in_DangerAmount *= 0.01f;

        float vibrationX = parentStartPos.x + Mathf.Clamp(Random.Range(-1f, 1f) * in_DangerAmount, -2f, 2f);
        float vibrationY = parentStartPos.y + Mathf.Clamp(Random.Range(-1f, 1f) * in_DangerAmount, -2f, 2f);

        Vector3 newPosition = new Vector3(vibrationX, vibrationY, transform.position.z);

        return newPosition;
    }

    void UpdateExplodedBool()
    {
        if (exploded)
            exploded = false;
        else
            exploded = !GetComponent<MeshRenderer>().enabled;

        print(exploded);
    }
}
