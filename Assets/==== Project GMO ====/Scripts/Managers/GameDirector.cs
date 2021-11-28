using System;
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

    [SerializeField] private List<GameObject> enemy = new List<GameObject>();

    [Header("Wave")]
    [SerializeField] private int wavePrepTime;
    [SerializeField] private int maxWaveCount;

    [Header("Wave Spawning")]
    [SerializeField] private int spawnCountGrowth;
    [SerializeField] private float spawnPaceGrowth;
    [SerializeField] private int spawnPackGrowth;

    private float spawnTime;
    public int currentWave { get; private set; }
    private int currentWaveEnemyCount = 0;
    private float currentSpawnPace = 0;
    private int currentSpawnPack = 0;

    private List<Enemy> currentEnemy = new List<Enemy>();
    private int enemyToSpawn;
    private int killCount = 0;

    public bool gameStarted { get; private set; }
    public float waitTime { get; private set; }
    public bool isPrepPhase { get; private set; }

    //====================Events================
    public event EventHandler OnStartGame;
    public event EventHandler OnEnterPrepPhase;
    public event EventHandler OnEnterRoundPhase;

    // Start is called before the first frame update
    void Start()
    {
        isPrepPhase = true;
        killCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnWave();
    }

    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            ResetWave();
            StartGameEvent(new EventArgs { });
        }
        else
        {
            if (isPrepPhase)
            {
                waitTime = Time.time + 3.5f;
            }
        }
       
    }

    private void SpawnWave()
    {
        bool enemyAwaitSpawn = currentWaveEnemyCount > 0 && spawnTime > 0 && currentWave < maxWaveCount;

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
        int maxPackCount = enemyToSpawn > currentSpawnPack ? currentSpawnPack : enemyToSpawn;
        int minPackCount = enemyToSpawn <= 0 ? 0 : 1;

        int randomPackCount = UnityEngine.Random.Range(minPackCount, maxPackCount);

        for (int i = 0; i < randomPackCount; i++)
        {
            GameObject enemyInstance = Instantiate(enemy[UnityEngine.Random.Range(0, enemy.Count)]);
            AddCurrentEnemy(enemyInstance.GetComponent<Enemy>());
            enemyInstance.GetComponent<EnemyTarget>().SetObjective(objectiveTarget);
            spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Count)].Spawn(enemyInstance);

            enemyToSpawn -= 1;
        }
    }

    public void AddCurrentEnemy(Enemy enemy)
    {
        currentEnemy.Add(enemy);
        enemy.OnDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.OnDeath -= OnEnemyDeath;

            currentEnemy.Remove(enemy);
            killCount += 1;
            CheckWaveComplete();
        }
    }

    private void CheckWaveComplete()
    {
        if(currentEnemy.Count == 0 && currentWaveEnemyCount == killCount)
        {
            WaitingForNextWave();
        }
    }

    private async void WaitingForNextWave()
    {
        EnterPrepPhaseEvent(new EventArgs { });

        waitTime = Time.time + wavePrepTime;

        isPrepPhase = true;

        while (Time.time < waitTime)
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
        isPrepPhase = false;
        EnterRoundPhase(new EventArgs { });

        enemyToSpawn = currentWaveEnemyCount;
        killCount = 0;
        currentEnemy.Clear();
    }

    private void SetCurrentWave()
    {
        currentWaveEnemyCount = spawnCountGrowth * currentWave;
        currentSpawnPace = 1 / (spawnPaceGrowth * currentWave);
        currentSpawnPack = spawnPackGrowth * currentWave;
    }

    private void StartGameEvent(EventArgs e)
    {
        OnStartGame?.Invoke(this, e);
    }

    private void EnterPrepPhaseEvent(EventArgs e)
    {
        OnEnterPrepPhase?.Invoke(this, e);
    }

    private void EnterRoundPhase(EventArgs e)
    {
        OnEnterRoundPhase?.Invoke(this, e);
    }
}
