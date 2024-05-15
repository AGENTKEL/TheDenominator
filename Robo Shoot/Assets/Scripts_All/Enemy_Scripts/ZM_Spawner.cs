using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZM_Spawner : MonoBehaviour
{
    [SerializeField] private float countdown;

    [SerializeField] public Wave[] waves;

    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private TextMeshProUGUI waveIndexText;
    [SerializeField] private TextMeshProUGUI countdownText;

    public int currentWaveIndex = 0;
    private bool readyToCountDown;

    private void Update()
    {
        if (currentWaveIndex >= waves.Length)
        {
            // If all waves are completed, repeat the last wave
            currentWaveIndex = waves.Length - 1;
        }

        if (readyToCountDown == true)
        {
            countdown -= Time.deltaTime;
            countdownText.text = "Отсчёт: " + Mathf.Ceil(countdown).ToString();
        }

        if (countdown <= 0)
        {
            readyToCountDown = false;
            countdown = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(SpawnWave());
            countdownText.text = "";
        }

        if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;

            if (currentWaveIndex < waves.Length - 1)
                currentWaveIndex++;
            else
                currentWaveIndex = waves.Length - 1;  // Reset to the last wave

            UpdateWaveIndexText();
        }
    }

    private void Start()
    {
        readyToCountDown = true;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }

    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
            {
                // Choose a random spawn point
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // Instantiate the enemy at the chosen spawn point
                EnemyNumberAi enemy = Instantiate(waves[currentWaveIndex].enemies[i], randomSpawnPoint.position, randomSpawnPoint.rotation);
                enemy.transform.SetParent(randomSpawnPoint);

                float randomSpawnTime = Random.Range(0, waves[currentWaveIndex].timeToNextEnemy);

                yield return new WaitForSeconds(randomSpawnTime);
            }
        }
    }

    public void AddSpawnPoints(Transform[] additionalSpawnPoints)
    {
        List<Transform> currentSpawnPoints = new List<Transform>(spawnPoints);
        currentSpawnPoints.AddRange(additionalSpawnPoints);
        spawnPoints = currentSpawnPoints.ToArray();
    }

    private void UpdateWaveIndexText()
    {
        if (waveIndexText != null)
            waveIndexText.text = "Волна: " + (currentWaveIndex + 1);
    }
}

[System.Serializable]
public class Wave
{
    public EnemyNumberAi[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}
