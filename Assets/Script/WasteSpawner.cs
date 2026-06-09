using System.Collections;
using UnityEngine;

public class WasteSpawner : MonoBehaviour
{
    public GameObject[] wastePrefabs;
    public Transform[] spawnPoints;
    public LevelManager levelManager; 

    private float baseSpawnInterval = 2.5f;
    private float spawnInterval;

    private float minSpawnInterval = 0.5f;
    void Start()
{
        if (levelManager == null) levelManager = Object.FindAnyObjectByType<LevelManager>();
        
        int currentLevel = GameMemory.currentLevelNumber > 0 ? GameMemory.currentLevelNumber : 1;
        
        // Calculate the interval once and store it in the field
        spawnInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - ((currentLevel - 1) * 0.15f));
        
        Debug.Log("Spawner using interval: " + spawnInterval);
        
        StartCoroutine(SpawnWasteRoutine());
}
    IEnumerator SpawnWasteRoutine()
{
        while (levelManager != null && !levelManager.gameStarted) 
        {
            yield return null; 
        }

        while (true) 
        {
            SpawnWaste();
            // Uses the variable calculated in Start()
            yield return new WaitForSeconds(spawnInterval); 
        }
}

    void SpawnWaste()
    {
        if (wastePrefabs.Length == 0 || spawnPoints.Length == 0) return;
        Instantiate(wastePrefabs[Random.Range(0, wastePrefabs.Length)], 
                    spawnPoints[Random.Range(0, spawnPoints.Length)].position, 
                    Quaternion.identity);
    }

    public void RegisterSortedItem()
    {
        Debug.Log("Waste removed from play.");
    }
}