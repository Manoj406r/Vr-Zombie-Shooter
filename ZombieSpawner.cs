using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombiePool
{
    public GameObject zombie; 
    public int poolSize;
    [HideInInspector]
    public Queue<GameObject> poolQueue = new Queue<GameObject>();
}

public class ZombieSpawner : MonoBehaviour
{
    public List<ZombiePool> zombiePools; 
    public List<Transform> spawnAreas; 
    public int maxActiveZombies = 20; 
    public float spawnInterval = 5f; 

    private int currentZombieCount = 0;

    void Start()
    {
        InitializePools();
        InvokeRepeating(nameof(SpawnZombies), spawnInterval, spawnInterval);
    }

    void InitializePools()
    {
        foreach (ZombiePool pool in zombiePools)
        {
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.zombie, Vector3.zero, Quaternion.identity);
                obj.SetActive(false); 
                pool.poolQueue.Enqueue(obj);
            }
        }
    }

    void SpawnZombies()
    {
        if (currentZombieCount >= maxActiveZombies) return;

        foreach (ZombiePool pool in zombiePools)
        {
            if (pool.poolQueue.Count > 0)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                GameObject zombie = pool.poolQueue.Dequeue();
                zombie.transform.position = spawnPosition;
                zombie.SetActive(true); 

                currentZombieCount++;
            }
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        if (spawnAreas.Count == 0)
        {
            Debug.LogWarning("No spawn areas defined.");
            return transform.position; 
        }

        
        int index = Random.Range(0, spawnAreas.Count);
        Transform selectedArea = spawnAreas[index];

        
        Vector3 randomPosition = new Vector3(
            Random.Range(selectedArea.position.x - 5f, selectedArea.position.x + 5f), 
            selectedArea.position.y, 
            Random.Range(selectedArea.position.z - 5f, selectedArea.position.z + 5f) 
        );

        return randomPosition; 
    }

    public void DeactivateZombie(GameObject zombie, ZombiePool pool)
    {
        zombie.SetActive(false);
        pool.poolQueue.Enqueue(zombie); 
        currentZombieCount--;
    }
}
