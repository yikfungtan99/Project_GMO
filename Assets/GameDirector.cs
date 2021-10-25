using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{

    [Header("Gameplay")]
    [SerializeField] private GameObject objectiveTarget;

    [Header("Spawning")]
    [SerializeField] private SpawnLocation playerSpawnLocation;

    [Tooltip("Location to spawn units")]
    [SerializeField] private List<SpawnLocation> spawnLocations = new List<SpawnLocation>();

    [SerializeField] private GameObject enemy;

    [SerializeField] private float spawnPace;
    [SerializeField] private int spawnPack;
    private float spawnTime;

    [SerializeField] private int waveCount;
    [SerializeField] private int spawnCount;
    private int currentSpawn = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawn = spawnCount;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnWave();
    }

    private void SpawnWave()
    {
        bool enemyAwaitSpawn = currentSpawn > 0 && spawnTime > 0;

        if (enemyAwaitSpawn)
        {
            spawnTime -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            spawnTime = spawnPace;
        }
    }

    public void SpawnEnemy()
    {
        int maxPackCount = currentSpawn > spawnPack ? spawnPack : currentSpawn;
        int minPackCount = currentSpawn <= 0 ? 0 : 1;

        Debug.Log(minPackCount);
        Debug.Log(maxPackCount);

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

        ResetWave();
    }

    private void ResetWave()
    {
        waveCount += 1;
        currentSpawn = waveCount * spawnCount;
        spawnTime = spawnPace;
    }
}
