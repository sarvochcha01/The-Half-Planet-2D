using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject creditsCanvas;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject bgCam;

    public bool isSettingsActive;


    private void Start()
    {
        creditsCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    private void Update()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        //else menuBgmSource.volume = 0;
    }
    public void LoadGameplay()
    {
        //FindObjectOfType<UIAudioManager>().Play("Click");
        StartCoroutine(LoadScene("Gameplay"));
        
    }

    public void LoadCinematic()
    {
        //FindObjectOfType<UIAudioManager>().Play("Click");
        StartCoroutine(LoadScene("Cinematic"));
    }

    public void OpenSettings()
    {
        //FindObjectOfType<UIAudioManager>().Play("Click");
        FindObjectOfType<GameManager>().LoadPlayerPrefs();
        settingsCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
        bgCam.SetActive(false);
        isSettingsActive = true;
    }

    public void CloseSettings()
    {
        //FindObjectOfType<UIAudioManager>().Play("Click");
        FindObjectOfType<GameManager>().SavePlayerPrefs();
        settingsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        bgCam.SetActive(true);
        isSettingsActive = false;
    }

    public void OpenCredits()
    {
        //FindObjectOfType<UIAudioManager>().Play("Click");
        creditsCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    public void CloseCredits()
    {
        //FindObjectOfType<UIAudioManager>().Play("Click");
        creditsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void QuitToDesktop()
    {
        //FindObjectOfType<UIAudioManager>().Play("Click");
        Application.Quit();
    }

    IEnumerator LoadScene(string scene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }
}
