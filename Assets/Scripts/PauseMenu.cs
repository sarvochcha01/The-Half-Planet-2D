using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isGamePaused;
    [SerializeField] private GameObject pauseMenuObject;

    [SerializeField] private GameObject settingsCanvasParent;

    public bool isSettingsActive;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (settingsCanvasParent == null)
        {
            settingsCanvasParent = GameObject.FindGameObjectWithTag("SettingsCanvas");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (isGamePaused)
        {
            Cursor.visible = true;
        }
        else Cursor.visible = false;
    }

    public void Resume()
    {
        pauseMenuObject.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1;
    }

    public void Pause()
    { 
        isGamePaused = true;
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void OpenSettings()
    {
        isSettingsActive = true;
        settingsCanvasParent.transform.GetChild(0).gameObject.SetActive(true);
        pauseMenuObject.SetActive(false);
    }

    public void CloseSettings()
    {
        isSettingsActive = false;
        settingsCanvasParent.transform.GetChild(0).gameObject.SetActive(false);
        pauseMenuObject.SetActive(true);
    }

    public void Quit()
    {
        Time.timeScale = 1;
        GameManager gm = FindObjectOfType<GameManager>();
        StartCoroutine(gm.GameFinished());
        gm.SetIsGameOver(true);
    }
}
