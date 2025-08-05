using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject animalPrefab;          
    public int maxAnimals = 5;
    public float spawnRadius = 10f;
    public float minDistanceToPlayer = 5f;

    private Transform player;
    private List<GameObject> spawnedAnimals = new List<GameObject>();

    void Start()
    {
        player = Camera.main.transform;

        if (animalPrefab == null)
        {
            Debug.LogError("Animal prefab not assigned!");
            enabled = false;
            return;
        }

        StartCoroutine(SpawnAnimalsPeriodically());
    }

    IEnumerator SpawnAnimalsPeriodically()
    {
        while (true)
        {
            CleanUpDestroyedAnimals();

            if (spawnedAnimals.Count < maxAnimals)
            {
                Vector3 spawnPos = GetRandomPositionAroundPlayer();
                float distance = Vector3.Distance(player.position, spawnPos);

                if (distance > minDistanceToPlayer)
                {
                    GameObject newAnimal = Instantiate(animalPrefab, spawnPos, Quaternion.identity);
                    FacePlayer(newAnimal.transform);
                    spawnedAnimals.Add(newAnimal);
                }
            }


            yield return new WaitForSeconds(3f);
        }
    }

    Vector3 GetRandomPositionAroundPlayer()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minDistanceToPlayer, spawnRadius);
        Vector3 spawnPosition = player.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        spawnPosition.y += 0.05f; // Slightly above ground

        return spawnPosition;
    }

    void FacePlayer(Transform target)
    {
        Vector3 direction = player.position - target.position;
        direction.y = 0;
        target.rotation = Quaternion.LookRotation(direction);
    }

    void CleanUpDestroyedAnimals()
    {
        // Remove null (destroyed) animals
        spawnedAnimals.RemoveAll(animal => animal == null);
    }
}
