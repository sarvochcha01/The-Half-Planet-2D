using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isGameOver;
    [SerializeField] private bool menubgmPlaying;
    [SerializeField] private bool gamebgmPlaying;
    [SerializeField] private bool gameoverbgmPlaying;

    [SerializeField] private static GameManager Instance;

    [SerializeField] private GameObject gameOverOverlay;

    [SerializeField] private bool gameOverCalled;

    [SerializeField] private Slider difficultySlider;
    public int difficulty = 1;
    [SerializeField] private bool diffSliderFound;

    [SerializeField] private Slider enemySpawnCountSlider;
    public int enemySpawnCount = 1;
    [SerializeField] private bool enemySpawnCountSliderFound;

    [SerializeField] private Slider uiVolSlider;
    public int uiVol = 100;
    [SerializeField] private bool uiVolSliderFound;

    [SerializeField] private Slider bgmVolSlider;
    public int bgmVol = 100;
    [SerializeField] private bool bgmVolSliderFound;

    [SerializeField] private Slider gunVolSlider;
    public int gunVol = 100;
    [SerializeField] private bool gunVolSliderFound;


    // PLAYER STATS
    public int totalDamageGiven;
    public int totalDamageTaken;

    public int totalHealthHealed;

    public int totalAmmoUsed;
    public int totalAmmoWasted;
    public int totalAmmoHitTarget;

    public int enemiesKilled;
    public int wavesSurvived;

    public string status;

    private void Start()
    {
        difficulty = 2;
        enemySpawnCount = 2;
        uiVol = 100;
        bgmVol = 100;
        gunVol = 100;

        menubgmPlaying = false;
        gamebgmPlaying = false;
        gameoverbgmPlaying = false;

        LoadPlayerPrefs();

        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
        FindObjectOfType<BGMAudioManager>().Play("MenuBGM");
    }

    public IEnumerator GameFinished()
    {
        yield return new WaitForSeconds(0.5f);
        //gameOverOverlay.GetComponent<Animator>().SetTrigger("GameOver");
        SceneManager.LoadScene("GameOver");
    }

    private void Update()
    {
        if (isGameOver && !gameOverCalled)
        {
            StartCoroutine(GameFinished());
            gameOverCalled = true;
        }

        //if (gameOverOverlay == null && SceneManager.GetActiveScene().name == "Gameplay")
        //{
        //    gameOverOverlay = GameObject.FindGameObjectWithTag("GOImage");
        //}

        bool isMenuScene = SceneManager.GetActiveScene().name == "Menu";
        bool isGameplayScene = SceneManager.GetActiveScene().name == "Gameplay";
        bool isGameOverScene = SceneManager.GetActiveScene().name == "GameOver";

        if (isMenuScene)
        {
            Cursor.visible = true;
            gameOverCalled = false;
            isGameOver = false;
            ResetPlayerStats();
        }

        if (isGameplayScene && !gamebgmPlaying)
        {
            FindObjectOfType<BGMAudioManager>().ResetSound();
            FindObjectOfType<BGMAudioManager>().Play("GameBGM");
            gamebgmPlaying = true;
            menubgmPlaying = false;
            gameoverbgmPlaying = false;

        }

        if (isMenuScene && !menubgmPlaying)
        {
            FindObjectOfType<BGMAudioManager>().ResetSound();
            FindObjectOfType<BGMAudioManager>().Play("MenuBGM");
            menubgmPlaying = true;
            gamebgmPlaying = false;
            gameoverbgmPlaying = false;
        }


        if (isGameOverScene && !gameoverbgmPlaying)
        {
            FindObjectOfType<BGMAudioManager>().ResetSound();
            FindObjectOfType<BGMAudioManager>().Play("GameOverBGM");
            gameoverbgmPlaying = true;
            gamebgmPlaying = false;
            menubgmPlaying = false;
        }

        //else Cursor.visible = false;

        if (isMenuScene && FindObjectOfType<ManageMenu>().isSettingsActive)
        {
            if (difficultySlider == null)
            {
                difficultySlider = GameObject.FindGameObjectWithTag("DiffSlider").GetComponent<Slider>();
                //difficulty = (int)difficultySlider.value;
            }

            if (enemySpawnCountSlider == null)
            {
                enemySpawnCountSlider = GameObject.FindGameObjectWithTag("EnemySlider").GetComponent<Slider>();
                //enemySpawnFrequency = (int) spawnFreqSlider.value;
            }

            if (uiVolSlider == null)
            {
                uiVolSlider = GameObject.FindGameObjectWithTag("UIVolslider").GetComponent<Slider>();
                //uiVol = (int)uiVolSlider.value;               
            }

            if (bgmVolSlider == null)
            {
                bgmVolSlider = GameObject.FindGameObjectWithTag("BGMVolSlider").GetComponent<Slider>();
                //bgmVol = (int)bgmVolSlider.value;              
            }

            if (gunVolSlider == null)
            {
                gunVolSlider = GameObject.FindGameObjectWithTag("GunVolSlider").GetComponent<Slider>();
                //gunVol = (int)gunVolSlider.value;               
            }

            difficultySlider.onValueChanged.AddListener(delegate { ChangeDifficulty(); });
            enemySpawnCountSlider.onValueChanged.AddListener(delegate { ChangeEnemySpawn(); });
            uiVolSlider.onValueChanged.AddListener(delegate { ChangeUIVol(); });
            bgmVolSlider.onValueChanged.AddListener(delegate { ChangeBGMVol(); });
            gunVolSlider.onValueChanged.AddListener(delegate { ChangeGunVol(); });
        }

        if (isGameplayScene && FindObjectOfType<PauseMenu>().isSettingsActive)
        {

        }
    }

    private void ChangeDifficulty()
    {
        difficulty = (int)difficultySlider.value;
    }

    private void ChangeEnemySpawn()
    {
        enemySpawnCount = (int)enemySpawnCountSlider.value;
    }

    private void ChangeUIVol()
    {
        uiVol = (int)uiVolSlider.value;
        //FindObjectOfType<UIAudioManager>().UpdateVol(uiVol);
    }

    private void ChangeBGMVol()
    {
        bgmVol = (int)bgmVolSlider.value;
        //FindObjectOfType<ManageMenu>().UpdateBGMVol(bgmVol);
    }

    private void ChangeGunVol()
    {
        gunVol = (int)gunVolSlider.value;
        //FindObjectOfType<GunAudioManager>().UpdateVol(gunVol);
    }

    public bool IsGameOver()
    { return isGameOver; }

    public void SetIsGameOver(bool val)
    { isGameOver = val; }

    public void LoadPlayerPrefs()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 2);
        difficultySlider.value = difficulty;

        enemySpawnCount = PlayerPrefs.GetInt("EnemySpawn", 1);
        enemySpawnCountSlider.value = enemySpawnCount;

        uiVol = PlayerPrefs.GetInt("UIVol", 100);
        uiVolSlider.value = uiVol;

        bgmVol = PlayerPrefs.GetInt("BGMVol", 100);
        bgmVolSlider.value = bgmVol;

        gunVol = PlayerPrefs.GetInt("GunVol", 100);
        gunVolSlider.value = gunVol;
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PlayerPrefs.SetInt("EnemySpawn", enemySpawnCount);
        PlayerPrefs.SetInt("UIVol", uiVol);
        PlayerPrefs.SetInt("BGMVol", bgmVol);
        PlayerPrefs.SetInt("GunVol", gunVol);
    }

    private void ResetPlayerStats()
    {
        totalDamageGiven = 0;
        totalDamageTaken = 0;

        totalHealthHealed = 0;

        totalAmmoUsed = 0;
        totalAmmoWasted = 0;
        totalAmmoHitTarget = 0;

        enemiesKilled = 0;
        wavesSurvived = 0;

        status = "";
    }
}

