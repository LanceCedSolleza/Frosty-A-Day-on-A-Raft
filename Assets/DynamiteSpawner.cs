using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteSpawner : MonoBehaviour
{
    public GameObject[] dynamitePrefabs; // Array of fish prefabs
    public float spawnInterval = 2f; // Time interval between spawns
    public float spawnXMin, spawnXMax, spawnYMin, spawnYMax; // boundaries for spawning

    public List<GameObject> activeDynamite = new List<GameObject>();

    void Start()
    {
        InvokeRepeating(nameof(SpawnDynamite), spawnInterval, spawnInterval);
    }

    void SpawnDynamite()
    {
        int randomIndex = Random.Range(0, dynamitePrefabs.Length);
        GameObject dynamite = Instantiate(dynamitePrefabs[randomIndex]);

        activeDynamite.Add(dynamite);

        // Random position
        float spawnX = Random.Range(spawnXMin, spawnXMax);
        float spawnY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        dynamite.transform.position = spawnPosition;

        // Random direction
        FishMovement fishMovement = dynamite.GetComponent<FishMovement>();
        fishMovement.SetDirection(spawnX < 0 ? Vector2.right : Vector2.left); // Left-to-right or vice versa
    }

    public void StopSpawningDynamite()
    {
        CancelInvoke();
    }

    public void StartSpawningDynamite()
    {
        InvokeRepeating(nameof(SpawnDynamite), spawnInterval, spawnInterval);
    }

    public void DestroyAllDynamite()
    {
        foreach (GameObject dynamite in activeDynamite)
        {
            if (dynamite != null)
            {
                Destroy(dynamite);
            }
        }

        // Clear the list after destroying all fishes
        activeDynamite.Clear();
    }
}
