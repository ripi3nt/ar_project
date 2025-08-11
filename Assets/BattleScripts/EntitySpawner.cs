using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EntitySpawner : MonoBehaviour
{
    private PetData petData;
    private EnemyData enemyData;

    public ARRaycastManager raycastManager; // Assign in Inspector
    public ARPlaneManager planeManager;
    public float spawnDistance = 0.5f;

    private bool spawned = false;

    void Start()
    {
        petData = BattleManager.Instance.selectedPet;
        enemyData = BattleManager.Instance.selectedEnemy;
    }

    void OnEnable()
    {
        planeManager.planesChanged += handlePlanes;
    }

    void handlePlanes(ARPlanesChangedEventArgs args)
    {
        if (spawned) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            Vector3 center = hitPose.position;
            Vector3 right = Camera.main.transform.right; // Use camera's right to decide orientation on the plane

            // Spawn positions
            Vector3 petPos = center - right * spawnDistance;
            Vector3 enemyPos = center + right * spawnDistance;

            // Instantiate
            GameObject pet = Instantiate(petData.petPrefab, petPos, Quaternion.LookRotation(enemyPos + petPos));
            GameObject enemy = Instantiate(enemyData.enemyPrefab, enemyPos, Quaternion.LookRotation(petPos - enemyPos));

            // Ensure they're upright
            pet.transform.up = hitPose.up;
            enemy.transform.up = hitPose.up;

            BattleSystem.Instance.start = true;
            spawned = true;
        }
    }
}
