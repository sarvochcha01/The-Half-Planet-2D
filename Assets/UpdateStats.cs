using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UpdateStats : MonoBehaviour
{
    [SerializeField] private float accuracyPercent;

    [SerializeField] private TextMeshProUGUI status;

    [SerializeField] private TextMeshProUGUI damageGiven;
    [SerializeField] private TextMeshProUGUI damageTaken;

    [SerializeField] private TextMeshProUGUI enemiesKilled;
    [SerializeField] private TextMeshProUGUI wavesSurvived;
    [SerializeField] private TextMeshProUGUI healthHealed;

    [SerializeField] private TextMeshProUGUI ammoUsed;
    [SerializeField] private TextMeshProUGUI ammoWasted;
    [SerializeField] private TextMeshProUGUI ammoHitTarget;
    [SerializeField] private TextMeshProUGUI accuracy;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool gameManagerFound;

    //BEST

    [SerializeField] private TextMeshProUGUI bestDamageGiven;
    [SerializeField] private TextMeshProUGUI bestDamageTaken;

    [SerializeField] private TextMeshProUGUI bestEnemiesKilled;
    [SerializeField] private TextMeshProUGUI bestWavesSurvived;
    [SerializeField] private TextMeshProUGUI bestHealthHealed;

    [SerializeField] private TextMeshProUGUI mostAmmoUsed;
    [SerializeField] private TextMeshProUGUI mostAmmoWasted;
    [SerializeField] private TextMeshProUGUI mostAmmoHitTarget;
    [SerializeField] private TextMeshProUGUI bestAccuracy;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerFound = false;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManagerFound)
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManagerFound = true;
        }

        DisplayCurrentStats();
        UpdateBestStats();
        DisplayBestStats();

    }

    private void DisplayCurrentStats()
    {
        status.text = gameManager.status;

        damageGiven.text = gameManager.totalDamageGiven.ToString();
        damageTaken.text = gameManager.totalDamageTaken.ToString();

        enemiesKilled.text = gameManager.enemiesKilled.ToString();
        wavesSurvived.text = gameManager.wavesSurvived.ToString();
        healthHealed.text = gameManager.totalHealthHealed.ToString();

        ammoUsed.text = gameManager.totalAmmoUsed.ToString();
        ammoWasted.text = gameManager.totalAmmoWasted.ToString();
        ammoHitTarget.text = gameManager.totalAmmoHitTarget.ToString();

        accuracyPercent = ((float)gameManager.totalAmmoHitTarget / gameManager.totalAmmoUsed * 100);
        accuracy.text = System.Math.Round(accuracyPercent, 2).ToString();
    }

    private void UpdateBestStats()
    {
        if (gameManager.totalDamageGiven > PlayerPrefs.GetInt("BestDamageGiven"))
        {
            PlayerPrefs.SetInt("BestDamageGiven", gameManager.totalDamageGiven);
        }

        if (gameManager.totalDamageTaken > PlayerPrefs.GetInt("BestDamageTaken"))
        {
            PlayerPrefs.SetInt("BestDamageTaken", gameManager.totalDamageTaken);
        }

        if (gameManager.enemiesKilled > PlayerPrefs.GetInt("BestEnemiesKilled"))
        {
            PlayerPrefs.SetInt("BestEnemiesKilled", gameManager.enemiesKilled);
        }

        if (gameManager.wavesSurvived > PlayerPrefs.GetInt("BestWavesSurvived"))
        {
            PlayerPrefs.SetInt("BestWavesSurvived", gameManager.wavesSurvived);
        }

        if (gameManager.totalHealthHealed > PlayerPrefs.GetInt("BestHealthHealed"))
        {
            PlayerPrefs.SetInt("BestHealthHealed", gameManager.totalHealthHealed);
        }

        if (gameManager.totalAmmoUsed > PlayerPrefs.GetInt("MostAmmoUsed"))
        {
            PlayerPrefs.SetInt("MostAmmoUsed", gameManager.totalAmmoUsed);
        }

        if (gameManager.totalAmmoWasted > PlayerPrefs.GetInt("MostAmmoWasted"))
        {
            PlayerPrefs.SetInt("MostAmmoWasted", gameManager.totalAmmoWasted);
        }

        if (gameManager.totalAmmoHitTarget > PlayerPrefs.GetInt("BestAmmoHitTarget"))
        {
            PlayerPrefs.SetInt("BestAmmoHitTarget", gameManager.totalAmmoHitTarget);
        }

        if (accuracyPercent > PlayerPrefs.GetFloat("BestAccuracyPercent"))
        {
            PlayerPrefs.SetFloat("BestAccuracyPercent", accuracyPercent);
        }

    }

    private void DisplayBestStats()
    {
        bestDamageGiven.text = "Highest: " + PlayerPrefs.GetInt("BestDamageGiven").ToString();
        bestDamageTaken.text = "Highest: " +PlayerPrefs.GetInt("BestDamageTaken").ToString();

        bestEnemiesKilled.text = "Highest " + PlayerPrefs.GetInt("BestEnemiesKilled").ToString();
        bestWavesSurvived.text = "Highest: " + PlayerPrefs.GetInt("BestWavesSurvived").ToString();
        bestHealthHealed.text = "Highest: " + PlayerPrefs.GetInt("BestHealthHealed").ToString();

        mostAmmoUsed.text = "Highest: " + PlayerPrefs.GetInt("MostAmmoUsed").ToString();
        mostAmmoWasted.text = "Highest: " + PlayerPrefs.GetInt("MostAmmoWasted").ToString();
        mostAmmoHitTarget.text = "Highest: " + PlayerPrefs.GetInt("BestAmmoHitTarget").ToString();

        bestAccuracy.text = "Highest: " + System.Math.Round(PlayerPrefs.GetFloat("BestAccuracyPercent"), 2).ToString();
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadMenuIEnum());
    }

    IEnumerator LoadMenuIEnum()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }
}
