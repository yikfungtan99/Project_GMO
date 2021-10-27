using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameDirector : MonoBehaviour
{

    [Header("Gameplay")]
    [SerializeField] private GameObject objectiveTarget;

    [Header("Spawning")]
    [SerializeField] private SpawnLocation playerSpawnLocation;

    [Tooltip("Location to spawn units")]
    [SerializeField] private List<SpawnLocation> spawnLocations = new List<SpawnLocation>();

    [SerializeField] private GameObject enemy; //Change to enemy storage

    [Header("Wave")]
    [SerializeField] private int wavePrepTime;
    [SerializeField] private int maxWaveCount;

    [Header("Wave Spawning")]
    [SerializeField] private int spawnCountGrowth;
    [SerializeField] private float spawnPaceGrowth;
    [SerializeField] private int spawnPackGrowth;
    
    private float spawnTime;

    private int currentWave = 1;
    private int currentSpawn = 0;
    private float currentSpawnPace = 0;
    private int currentSpawnPack = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentWave();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnWave();
    }

    private void SpawnWave()
    {
        bool enemyAwaitSpawn = currentSpawn > 0 && spawnTime > 0 && currentWave < maxWaveCount;

        if (enemyAwaitSpawn)
        {
            spawnTime -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            spawnTime = currentSpawnPace;
        }
    }

    public void SpawnEnemy()
    {
        int maxPackCount = currentSpawn > currentSpawnPack ? currentSpawnPack : currentSpawn;
        int minPackCount = currentSpawn <= 0 ? 0 : 1;

        int randomPackCount = Random.Range(minPackCount, maxPackCount);

        bool lastPack = currentSpawn - randomPackCount == 0;

        for (int i = 0; i < randomPackCount; i++)
        {
            bool lastEnemy = i == randomPackCount - 1;

            GameObject enemyInstance = Instantiate(enemy);
            enemyInstance.GetComponent<EnemyTarget>().SetObjective(objectiveTarget); 

            if (lastPack && lastEnemy) 
            {
                enemyInstance.GetComponent<Enemy>().OnDeath += WaveComplete;
            }

            spawnLocations[Random.Range(0, spawnLocations.Count)].Spawn(enemyInstance);

            currentSpawn -= 1;
        }
    }

    private void WaveComplete(object sender, System.EventArgs e)
    {
        Enemy enemy = sender as Enemy;
        enemy.OnDeath -= WaveComplete;

        WaitingForNextWave();
    }

    private async void WaitingForNextWave()
    {
        float waitTime = Time.time + wavePrepTime;

        while(Time.time < waitTime)
        {
            await Task.Yield();
        }

        ResetWave();
    }

    private void ResetWave()
    {
        currentWave += 1;
        SetCurrentWave();
        spawnTime = currentSpawnPace;
    }

    private void SetCurrentWave()
    {
        currentSpawn = spawnCountGrowth * currentWave;
        currentSpawnPace = 1/(spawnPaceGrowth * currentWave);
        currentSpawnPack = spawnPackGrowth * currentWave;
    }
}
