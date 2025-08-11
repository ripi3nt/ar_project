using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Enemy : MonoBehaviour
{
    [Header("Pet Settings")]
    public GameObject petPrefab;
    public GameObject circlePrefab;
    public float minDistanceBetweenPets = 1.5f;
    public float spawnInterval = 3f;

    [Header("AR Settings")]
    private ARPlaneManager planeManager;

    private List<GameObject> spawnedPets = new List<GameObject>();

    void Start()
    {
        planeManager = FindObjectsByType<ARPlaneManager>(FindObjectsSortMode.None)[0];
        // Auto-assign ARPlaneManager if not set
        if (planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();
            if (planeManager == null)
            {
                Debug.LogError("ARPlaneManager not found!");
                enabled = false;
                return;
            }
        }

        StartCoroutine(SpawnPetsGradually());
    }

    IEnumerator SpawnPetsGradually()
    {
        while (true)
        {
            List<ARPlane> planes = new List<ARPlane>();
            foreach (var plane in planeManager.trackables)
                planes.Add(plane);

            foreach (var plane in planes)
            {
                Vector3? spawnPos = GetRandomPointOnPlane(plane);
                if (spawnPos == null)
                {
                    continue;
                }

                if (IsFarFromOtherPets(spawnPos.GetValueOrDefault()))
                {
                    GameObject pet = Instantiate(petPrefab, spawnPos.GetValueOrDefault(), Quaternion.identity);
                    pet.tag = "Enemy";

                    // Add circle indicator
                    if (circlePrefab != null)
                    {
                        GameObject indicator = Instantiate(circlePrefab, pet.transform);
                        indicator.transform.localPosition = Vector3.zero;
                        indicator.SetActive(false);

                        EnemyIndicatorSimple indicatorScript = pet.AddComponent<EnemyIndicatorSimple>();
                        indicatorScript.circleIndicator = indicator;
                    }

                    spawnedPets.Add(pet);
                    yield return new WaitForSeconds(spawnInterval);
                }
            }

            yield return null;
        }
    }

    Vector3? GetRandomPointOnPlane(ARPlane plane)
    {
        if (plane == null)
        {
            return null;
        }
        Vector2 randomInPlane = Random.insideUnitCircle * 0.5f;
        Vector3 worldPos = plane.transform.TransformPoint(new Vector3(randomInPlane.x, 0, randomInPlane.y));
        worldPos.y += 0.01f;
        return worldPos;
    }

    bool IsFarFromOtherPets(Vector3 position)
    {
        foreach (GameObject pet in spawnedPets)
        {
            if (pet != null && Vector3.Distance(position, pet.transform.position) < minDistanceBetweenPets)
                return false;
        }
        return true;
    }
}
