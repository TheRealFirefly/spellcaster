using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public GameObject[] enemies;

    public int currentWave = 0;
    public float growthFactor = 1.15f;
    public bool spawningFinished = false;
    public float waveDuration = 30f;
    public float waveTimer = 0f;

    public int enemiesKilled = 0;

    public int baseEnemies = 5;

    public int level = 0;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        TickManager.instance.OnTick += Tick;
    }

    private void OnDisable()
    {
        TickManager.instance.OnTick -= Tick;
    }

    private void Tick()
    {
        if (GameManager.instance.gameStarted)
        {
            if (waveTimer > 0f)
            {
                waveTimer -= 1f;
            }
            if (spawningFinished && (baseEnemies + currentWave) - enemiesKilled == 0)
            {
                OpenShop();
            }
        }
    }
    private void OpenShop()
    { 
        ShopManager.instance.OpenShop(level);
    }
    public void StartNextWave()
    {
        currentWave++;

        if (currentWave % 5 == 0 && level !=10) level++;

        waveTimer = waveDuration;

        Debug.Log($"Wave {currentWave} startet!");

        int enemiesToSpawn = baseEnemies + currentWave;
        spawningFinished = false;
        enemiesKilled = 0;
        StartCoroutine(SpawnEnemiesOverTime(enemiesToSpawn));
    }

    private IEnumerator SpawnEnemiesOverTime(int count)
    {
        yield return new WaitForSeconds(1);
        float interval = waveDuration / count;

        for (int i = 0; i < count; i++)
        {
            SpawnEnemyRequest();
            yield return new WaitForSeconds(interval);
            Debug.Log("Gegner gespawned");
        }
        spawningFinished = true;

    }
    public void SpawnEnemyRequest()
    {
        int index = Random.Range(0, enemies.Length);
        GameObject prefab = enemies[index];

        Vector3 playerPos = PlayerStats.instance.transform.position;

        float minDistance = 7f; // Mindestabstand zum Spieler
        float spawnRadius = 10f; // Maximaler Abstand vom Spieler

        Vector3 spawnPos;
        int tries = 0;
        int maxTries = 20;

        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            spawnPos = playerPos + new Vector3(randomCircle.x, randomCircle.y, 0f);

            spawnPos.x = Mathf.Clamp(spawnPos.x, 4, 48);
            spawnPos.y = Mathf.Clamp(spawnPos.y, 4, 48);

            tries++;
        }
        // Wiederholen, wenn zu nah am Spieler oder auf Blocker
        while ((Vector3.Distance(spawnPos, playerPos) < minDistance
               || Physics2D.OverlapCircle(spawnPos, 0.3f)) && tries < maxTries);

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
        EnemyStats stats = obj.GetComponent<EnemyStats>();

        float mult = Mathf.Pow(growthFactor, currentWave - 1);
        float Speedmult = Mathf.Pow(1.05f, currentWave - 1);

        stats.InitializeStats(
            stats.HP, mult,
            stats.damage, mult,
            stats.speed, Speedmult
        );

    }


    public void NotifyEnemyKilled()
    {
        enemiesKilled++;
        Debug.Log("Gegner getötet");
    }

}
