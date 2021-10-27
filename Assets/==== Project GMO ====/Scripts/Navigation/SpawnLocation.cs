using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    private List<Transform> remainingSpawnpoint = new List<Transform>();

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i));
        }

        if(spawnPoints.Count == 0)
        {
            print("No spawnpoint(s) assigned!");

            spawnPoints.Add(transform);
        }

        remainingSpawnpoint = spawnPoints.ToList();
    }

    public void Spawn(GameObject spawn)
    {
        Transform selectedSpawnPoint = remainingSpawnpoint[Random.Range(0, remainingSpawnpoint.Count)];
        spawn.transform.position = selectedSpawnPoint.position;
        UseSpawnPoint(selectedSpawnPoint);
    }

    public void Spawn(GameObject spawn, Transform spawnPosition)
    {
        Transform selectedSpawnPoint = spawnPosition;
        spawn.transform.position = selectedSpawnPoint.position;
    }

    public void Spawn(List<GameObject> spawns)
    {
        foreach (GameObject spawn in spawns)
        {
            Spawn(spawn);
        }
    }

    private void UseSpawnPoint(Transform spawnPoint)
    {
        remainingSpawnpoint.Remove(spawnPoint);

        if(remainingSpawnpoint.Count <= 0)
        {
            remainingSpawnpoint = spawnPoints.ToList();
        }
    }

}