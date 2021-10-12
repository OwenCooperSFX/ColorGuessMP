using UnityEngine;

public class ColorObject : MonoBehaviour
{
    public ColorOption currentColor;
    private GameObject colorLightGO;
    private Light colorLight;

    private float timer = 0;
    private float lightFlashTime = .2f;
    private bool bIsLightOn = false;

    [SerializeField] private float defaultMaxIntensity = 5.0f;
    private float maxIntensity;

    [SerializeField] private float range = 1.8f;

    private void OnEnable()
    {
        //GameManager.Instance.OnPlayerInputCorrect += 
    }

    private void Awake()
    {
        // Light Construction
        colorLightGO = new GameObject();
        colorLightGO.transform.parent = transform;
        colorLightGO.transform.localPosition = new Vector3(0, 0, -.75f);
        colorLightGO.name = colorLightGO.transform.parent.name + " Light";

        colorLight = colorLightGO.AddComponent<Light>();
        colorLight.intensity = 0;
        colorLight.range = range;

        maxIntensity = defaultMaxIntensity;

        timer = lightFlashTime;
    }

    private void Update()
    {
        if (timer < lightFlashTime)
        {
            if (!bIsLightOn)
                bIsLightOn = true;

            if (colorLight.intensity < maxIntensity)
                colorLight.intensity += (maxIntensity / lightFlashTime) * Time.deltaTime;

            timer += Time.deltaTime;
        }
        else if (bIsLightOn && name != "ColorPrompt")
        {
            EndLight();
        }
    }
    public void FlashButtonLight()
    {
        DoLighting();
    }

    private void EndLight()
    {
        if (colorLight.intensity > 0)
        {
            colorLight.intensity -= (2 * maxIntensity / lightFlashTime) * Time.deltaTime;
        }
        else if (bIsLightOn)
        {
            bIsLightOn = false;
        }
    }

    public void LightColorPrompt()
    {
        colorLight.intensity = 0;
        DoLighting();
    }

    private void DoLighting()
    {
        timer = 0;

        if (colorLight)
        {
            maxIntensity = defaultMaxIntensity;

            switch (currentColor)
            {
                case ColorOption.blue:
                    colorLight.color = Color.blue;
                    maxIntensity += (.5f * defaultMaxIntensity);
                    break;
                case ColorOption.green:
                    colorLight.color = Color.green;
                    maxIntensity -= (.25f * defaultMaxIntensity);
                    break;
                case ColorOption.red:
                    colorLight.color = Color.red;
                    break;
                case ColorOption.yellow:
                    colorLight.color = Color.yellow;
                    maxIntensity -= (.25f * defaultMaxIntensity);
                    break;
                case ColorOption.invalid:
                    Debug.LogWarning("Invalid color!");
                    break;
            }
        }
    }
}
