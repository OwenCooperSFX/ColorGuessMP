using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    float musicVolume;
    float sfxVolume;

    [SerializeField] GameObject screenMainMenu;
    [SerializeField] GameObject screenSettings;

    public void OpenSettingsMenu()
    {
        StartCoroutine(ChangeMenuWithDelay(screenMainMenu, screenSettings, .3f));
    }

    public void OpenMainMenu()
    {
        StartCoroutine(ChangeMenuWithDelay(screenSettings, screenMainMenu, .3f));
    }

    public void QuitGame()
    {
        
    }

    public void SetMusicVolume(float inValue)
    {
        SetBusVolume("Music_Volume", musicVolume, inValue);
    }

    public void SetSfxVolume(float inValue)
    {
        SetBusVolume("SFX_Volume", sfxVolume, inValue);
    }

    private void SetBusVolume(string paramName, float volumeFloat, float inValue)
    {
        if (audioMixer)
        {
            // Divide currentValue by (minimumValue/currentValue) to get more linear response

            volumeFloat = inValue / (-80/inValue);

            audioMixer.SetFloat(paramName, volumeFloat);
        }
    }

    private void ChangeMenu(GameObject currentMenu, GameObject nextMenu)
    {
        if (!nextMenu.activeSelf)
        {
            nextMenu.SetActive(true);
        }

        if (currentMenu.activeSelf)
        {
            currentMenu.SetActive(false);
        }
    }

    IEnumerator ChangeMenuWithDelay(GameObject currentMenu, GameObject nextMenu, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        ChangeMenu(currentMenu, nextMenu);
    }
}
