using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;      // The platform prefab to spawn
    public float platformWidthRange = 6f;  // Range for random horizontal position of platforms (from -6 to +6)
    public float platformHeight = 4f;      // Vertical distance between platforms
    public int initialPlatformCount = 10;  // Number of platforms to spawn at the start

    private float lastSpawnHeight;         // Tracks the height of the last spawned platform
    private Transform playerTransform;     // Reference to the player

    void Start()
    {
        // Find the player in the scene
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Initialize the first few platforms
        lastSpawnHeight = playerTransform.position.y;
        SpawnInitialPlatforms();
    }

    void Update()
    {
        // Continuously spawn platforms ahead of the player
        while (lastSpawnHeight < playerTransform.position.y + 10f)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        // Randomize the horizontal position of the new platform relative to the player's position
        float randomX = Random.Range(playerTransform.position.x - platformWidthRange, playerTransform.position.x + platformWidthRange);

        // Increment the height for the next platform
        lastSpawnHeight += platformHeight;

        // Set the spawn position for the platform
        Vector3 spawnPosition = new Vector3(randomX, lastSpawnHeight, 0);

        // Instantiate the platform at the calculated position
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnInitialPlatforms()
    {
        // Spawn some initial platforms at the start of the game
        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }
}
