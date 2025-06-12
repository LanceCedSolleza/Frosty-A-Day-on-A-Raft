using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    public GameObject[] minePrefabs; // Array of fish prefabs
    public float spawnInterval = 2f; // Time interval between spawns
    public float spawnXMin, spawnXMax, spawnYMin, spawnYMax; // boundaries for spawning


    public List<GameObject> activeMines = new List<GameObject>();

    void Start()
    {   
    }

    void SpawnMines()
    {
        int randomIndex = Random.Range(0, minePrefabs.Length);
        GameObject mines = Instantiate(minePrefabs[randomIndex]);

        activeMines.Add(mines);

        // Random position
        float spawnX = Random.Range(spawnXMin, spawnXMax);
        float spawnY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        mines.transform.position = spawnPosition;

        // Random direction
        FishMovement fishMovement = mines.GetComponent<FishMovement>();
        fishMovement.SetDirection(spawnX < 0 ? Vector2.right : Vector2.left); // Left-to-right or vice versa
    }

    public void StopSpawningMines() {
        CancelInvoke();
    }

    public void StartSpawningMines() {
       InvokeRepeating(nameof(SpawnMines), spawnInterval, spawnInterval);
    }

    public void DestroyAllMines()
    {
        foreach (GameObject mines in activeMines)
        {
            if (mines != null)
            {
                Destroy(mines);
            }
        }

        // Clear the list after destroying all fishes
        activeMines.Clear();
    }
}
