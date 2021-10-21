using UnityEngine;

public class ScreenshakeController : MonoBehaviour
{
    public static ScreenshakeController Instance;

    private Vector3 startPos;

    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;
    public float rotationMultiplier = 1f;

    //IEnumerator Shake(float duration, float magnitude)
    //{
    //    Vector3 originalPos = transform.localPosition;

    //    float elapsed = 0f;

    //    while(elapsed < duration)
    //    {
    //        float x = Random.Range(-1f, 1f) * magnitude;
    //        float y = Random.Range(-1f, 1f) * magnitude;

    //        transform.localPosition
    //    }
    //}

    void Awake()
    {
        Init();

        startPos = transform.position;
    }

    void Init()
    {
        // Singleton logic
        if (Instance && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f, 1f));
        }

        float moveX = Mathf.MoveTowards(transform.position.x, startPos.x, shakeFadeTime);
        float moveY = Mathf.MoveTowards(transform.position.y, startPos.y, shakeFadeTime);

        transform.position = new Vector3(moveX, moveY, startPos.z);

        //transform.position = Vector3.zero;
        //transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;

        shakeRotation = power * rotationMultiplier;
    }
}
