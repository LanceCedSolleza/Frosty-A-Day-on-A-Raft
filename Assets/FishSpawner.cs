using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject[] fishPrefabs; // Array of fish prefabs
    public float spawnInterval = 2f; // Time interval between spawns
    public float spawnXMin, spawnXMax, spawnYMin, spawnYMax; // boundaries for spawning

    public List<GameObject> activeFishes = new List<GameObject>();
    void Start()
    {
        InvokeRepeating(nameof(SpawnFish), spawnInterval, spawnInterval);
    }

    void SpawnFish()
    {
        int randomIndex = Random.Range(0, fishPrefabs.Length);
        GameObject fish = Instantiate(fishPrefabs[randomIndex]);

        activeFishes.Add(fish);

        // Random position
        float spawnX = Random.Range(spawnXMin, spawnXMax);
        float spawnY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        fish.transform.position = spawnPosition;

        // Random direction
        FishMovement fishMovement = fish.GetComponent<FishMovement>();
        fishMovement.SetDirection(spawnX < 0 ? Vector2.right : Vector2.left); // Left-to-right or vice versa
    }

    public void StopSpawningFish()
    {
        CancelInvoke();
    }

    public void StartSpawningFish()
    {
        InvokeRepeating(nameof(SpawnFish), spawnInterval, spawnInterval);
    }

    public void DestroyAllFishes()
    {
        foreach (GameObject fish in activeFishes)
        {
            if (fish != null)
            {
                Destroy(fish);
            }
        }

        // Clear the list after destroying all fishes
        activeFishes.Clear();
    }

}
