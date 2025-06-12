using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerPrefabs; // Array of fish prefabs
    public float spawnInterval = 2f; // Time interval between spawns
    public float spawnXMin, spawnXMax, spawnYMin, spawnYMax; // boundaries for spawning

    public List<GameObject> activePowers = new List<GameObject>();
    void Start()
    {
        InvokeRepeating(nameof(SpawnPower), spawnInterval, spawnInterval);
    }

    void SpawnPower()
    {
        int randomIndex = Random.Range(0, powerPrefabs.Length);
        GameObject power = Instantiate(powerPrefabs[randomIndex]);

        activePowers.Add(power);

        // Random position
        float spawnX = Random.Range(spawnXMin, spawnXMax);
        float spawnY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        power.transform.position = spawnPosition;

        // Random direction
        FishMovement powerMovement = power.GetComponent<FishMovement>();
        powerMovement.SetDirection(spawnX < 0 ? Vector2.right : Vector2.left); // Left-to-right or vice versa
    }

    public void StopSpawningPower()
    {
        CancelInvoke();
    }

    public void StartSpawningPower()
    {
        InvokeRepeating(nameof(SpawnPower), spawnInterval, spawnInterval);
    }

    public void DestroyAllPowers()
    {
        foreach (GameObject power in activePowers)
        {
            if (power != null)
            {
                Destroy(power);
            }
        }

        // Clear the list after destroying all fishes
        activePowers.Clear();
    }
}
