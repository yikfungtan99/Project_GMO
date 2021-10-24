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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        GameObject enemyInstance = Instantiate(enemy);

        enemyInstance.GetComponent<EnemyTarget>().SetObjective(objectiveTarget);

        spawnLocations[Random.Range(0, spawnLocations.Count)].Spawn(enemyInstance);
    }
}
