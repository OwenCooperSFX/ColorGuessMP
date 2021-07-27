using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    // Stupid way of doing this for now. Keep alive for set time limit.

    float timer = 0;
    public float lifetime = 1;

    private void OnEnable()
    {
        timer = 0;
    }

    void Update()
    {
        if (timer < lifetime)
            timer += Time.deltaTime;
        else
            gameObject.SetActive(false);
    }
}
