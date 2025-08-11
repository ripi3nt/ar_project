using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI Button
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Enemy : MonoBehaviour
{
    [Header("Pet Settings")]
    public GameObject petPrefab;
    public float minDistanceBetweenPets = 1.5f;
    public float spawnInterval = 3f;

    [Header("AR Settings")]
    public ARPlaneManager planeManager;

    [Header("Interaction Settings")]
    public float interactDistance = 0.5f; // Distance to show button
    public Button interactButton;         // Reference to UI Button
    public Transform playerCamera;        // AR Camera or Main Camera

    private List<GameObject> spawnedPets = new List<GameObject>();
    private bool isSpawning = false;

    void Start()
    {
        // Auto-assign ARPlaneManager if not set
        if (planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();
            if (planeManager != null)
            {
                Debug.Log("ARPlaneManager auto-assigned.");
            }
        }

        if (planeManager == null)
        {
            Debug.LogError("ARPlaneManager not assigned and not found.");
            enabled = false;
            return;
        }

        // Hide button at start and link click event
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteractButtonClicked);
        }

        // Try to assign camera immediately (may still be null)
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        StartCoroutine(SpawnPetsGradually());
    }

    void Update()
    {
        // If camera is still not assigned, try again
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        // If no camera, skip logic
        if (playerCamera == null) return;

        GameObject nearestPet = GetNearestPet();

        if (nearestPet != null && Vector3.Distance(playerCamera.position, nearestPet.transform.position) <= interactDistance)
        {
            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(false);
            }
        }
    }

    GameObject GetNearestPet()
    {
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var pet in spawnedPets)
        {
            if (pet == null) continue;
            float dist = Vector3.Distance(playerCamera.position, pet.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = pet;
            }
        }
        return nearest;
    }

    void OnInteractButtonClicked()
    {
        GameObject nearestPet = GetNearestPet();
        if (nearestPet != null)
        {
            Debug.Log("Interacted with: " + nearestPet.name);
            Destroy(nearestPet); // Example: remove the pet
        }
    }

    IEnumerator SpawnPetsGradually()
    {
        isSpawning = true;

        while (true)
        {
            // Copy planes into our own list to avoid modification errors
            List<ARPlane> planes = new List<ARPlane>();
            foreach (var plane in planeManager.trackables)
            {
                planes.Add(plane);
            }

            foreach (var plane in planes)
            {
                Vector3 spawnPos = GetRandomPointOnPlane(plane);

                if (IsFarFromOtherPets(spawnPos))
                {
                    GameObject pet = Instantiate(petPrefab, spawnPos, Quaternion.identity);
                    pet.tag = "Enemy";
                    spawnedPets.Add(pet);
                    yield return new WaitForSeconds(spawnInterval);
                }
            }

            yield return null;
        }
    }

    Vector3 GetRandomPointOnPlane(ARPlane plane)
    {
        Vector2 randomInPlane = Random.insideUnitCircle * 0.5f;
        Vector3 worldPos = plane.transform.TransformPoint(new Vector3(randomInPlane.x, 0, randomInPlane.y));
        worldPos.y += 0.01f; // Slightly above the plane
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
