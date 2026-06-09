using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndlessWasteSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] wastePrefabs;
    public Transform spawnPoint;
    public float initialSpawnInterval = 2.0f;
    public float minSpawnInterval = 0.5f; // Fastest the spawning will go
    public float intervalDecreaseAmount = 0.2f; // How much faster it gets every 30 seconds

    [Header("Conveyor Settings")]
    public EndlessConveyor conveyorScript; // Drag your Conveyor script here
    public float speedIncreaseAmount = 0.5f; // How much the belt speeds up every 30 seconds
    public float maxConveyorSpeed = 6.0f; // Maximum speed limit for the conveyor belt

    [Header("Tracking")]
    private float currentSpawnInterval;
    private float difficultyTimer = 0f;
    private const float difficultyInterval = 30f; // 30 seconds milestone

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;

        // Save current scene for your Retry button
        GameMemory.lastPlayedLevel = SceneManager.GetActiveScene().name;

        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        // Track elapsed time to speed up components every 30 seconds
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= difficultyInterval)
        {
            difficultyTimer = 0f; // Reset milestone tracker
            IncreaseDifficulty();
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnWaste();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    void SpawnWaste()
    {
        if (wastePrefabs.Length == 0 || spawnPoint == null) return;
        int randomIndex = Random.Range(0, wastePrefabs.Length);
        Instantiate(wastePrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
    }

    void IncreaseDifficulty()
    {
        // 1. Make spawning faster, capped by the minSpawnInterval limit
        if (currentSpawnInterval > minSpawnInterval)
        {
            currentSpawnInterval -= intervalDecreaseAmount;
            currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnInterval);
        }

        // 2. Make conveyor belt faster, capped by the maxConveyorSpeed limit
        if (conveyorScript != null)
        {
            if (conveyorScript.speed < maxConveyorSpeed)
            {
                conveyorScript.speed += speedIncreaseAmount;
                conveyorScript.speed = Mathf.Min(conveyorScript.speed, maxConveyorSpeed);
            }
        }

        Debug.Log($"30 Seconds Passed! Difficulty Increased. New Interval: {currentSpawnInterval}s, New Speed: {(conveyorScript != null ? conveyorScript.speed.ToString() : "N/A")}");
    }
}