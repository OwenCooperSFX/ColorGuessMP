using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] AudioMixer audioMixer;

    float musicVolume;
    float sfxVolume;

    [SerializeField] GameObject screenMainMenu;
    [SerializeField] GameObject screenHowTo;
    [SerializeField] GameObject screenSettings;

    private GameObject currentScreen;

    [SerializeField] float menuTransitionTime = 0.5f;


    private void Awake()
    {
        Init();

        currentScreen = screenMainMenu;
    }
    void Init()
    {
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


    public void OpenMainMenu()
    {
        StartCoroutine(ChangeMenuWithDelay(currentScreen, screenMainMenu, menuTransitionTime));
    }

    public void OpenSettingsMenu()
    {
        StartCoroutine(ChangeMenuWithDelay(currentScreen, screenSettings, menuTransitionTime));
    }

    public void OpenHowToMenu()
    {
        StartCoroutine(ChangeMenuWithDelay(currentScreen, screenHowTo, menuTransitionTime));
    }

    public void QuitGame()
    {
        QuitWithDelay(menuTransitionTime);
    }

    public void SetMusicVolume(float inValue)
    {
        SetBusVolume("Music_Volume", musicVolume, inValue);
    }

    public void SetSfxVolume(float inValue)
    {
        SetBusVolume("SFX_Volume", sfxVolume, inValue);
    }

    private void SetBusVolume(string _paramName, float _volumeFloat, float _value)
    {
        if (audioMixer)
        {
            // Divide currentValue by (minimumValue/currentValue) to get more linear response

            _volumeFloat = _value / (-80/_value);

            audioMixer.SetFloat(_paramName, _volumeFloat);
        }
    }

    private void ChangeMenu(GameObject _currentMenu, GameObject _nextMenu)
    {
        // Enable the target menu
        if (!_nextMenu.activeSelf)
        {
            _nextMenu.SetActive(true);
        }

        // Disable the current menu
        if (_currentMenu.activeSelf)
        {
            _currentMenu.SetActive(false);
        }

        // Store reference to the target menu
        if (currentScreen != _nextMenu)
        {
            currentScreen = _nextMenu;
        }
    }

    IEnumerator ChangeMenuWithDelay(GameObject _currentMenu, GameObject _nextMenu, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);
        ChangeMenu(_currentMenu, _nextMenu);
    }

    IEnumerator QuitWithDelay(float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);
        Application.Quit();
    }
}
