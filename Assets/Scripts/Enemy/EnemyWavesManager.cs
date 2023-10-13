using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWavesManager : MonoBehaviour
{
    [SerializeField] private int waveCount;

    [SerializeField] private EnemySpawner[] enemySpawners = new EnemySpawner[15];

    [SerializeField] private float timer;
    [SerializeField] private float waveInterval;

    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private bool startWave;

    [SerializeField] private bool isFirstWave;

    [SerializeField] private MazeGenerator mazeGenerator;

    private void Start()
    {
        waveTimerText.text = "";
        StartCoroutine(GetEnemySpawners());
        waveCount = 1;
        timer = 60;
        startWave = false;
        isFirstWave = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (startWave)
        {
            timer -= Time.deltaTime;
            waveTimerText.text = System.MathF.Round(timer, 0).ToString();
            if (timer <= 0)
            {
                SetSpawnerBool(false);
                StartCoroutine(StartWave());
                timer = 60;
            }
        }
    }

    IEnumerator GetEnemySpawners()
    {
        yield return new WaitForSeconds(5);
        if (mazeGenerator.ShouldSpawn())
        {
            enemySpawners = FindObjectsOfType<EnemySpawner>();
            startWave = true;
            StartCoroutine(StartWave());
        }
            
    }

    IEnumerator StartWave()
    {
        yield return new WaitForSeconds(1);
        int count = 0;
        if (!IsMajorWave())
        {
            count = 1;
            FindObjectOfType<UINotificationManager>().SetText("Wave " + waveCount + " incoming"); 
        }
        else 
        {
            count = 2;
            FindObjectOfType<UINotificationManager>().SetText("Major wave incoming"); 
        }
        isFirstWave = false;
        waveCount++;
        FindObjectOfType<GameManager>().wavesSurvived = waveCount - 1;
        foreach (var spawner in enemySpawners)
        {
            spawner.SpawnEnemies(count);
        }
    }

    void SetSpawnerBool(bool value)
    {
        foreach (var spawner in enemySpawners)
        {
            spawner.SetSpawnEnemyBool(value);
        }
    }

    private bool IsMajorWave()
    {
        int random = Random.Range(0, 100);
        Debug.Log(random);
        if (random < 20 && !isFirstWave)
        {
            return true;
        }

        return false;
    }

    
}
