using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EntitySpawner : MonoBehaviour
{
    private GameObject petData;
    private GameObject enemyData;

    private ARRaycastManager raycastManager; // Assign in Inspector
    private ARPlaneManager planeManager;
    public float spawnDistance = 0.5f;

    private bool spawned = false;

    void Start()
    {
        petData = BattleManager.Instance.selectedPet;
        enemyData = BattleManager.Instance.selectedEnemy;

    }

    void OnEnable()
    {
        raycastManager = FindObjectsByType<ARRaycastManager>(FindObjectsSortMode.None)[0];
        planeManager = FindObjectsByType<ARPlaneManager>(FindObjectsSortMode.None)[0];

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
            petData = BattleManager.Instance.selectedPet;
            enemyData = BattleManager.Instance.selectedEnemy;
            petData.transform.position = petPos;
            enemyData.transform.position = enemyPos;
            petData.transform.LookAt(enemyPos, hitPose.up);
            enemyData.transform.LookAt(petPos, hitPose.up);

            enemyData.GetComponent<EnemyIndicatorSimple>().SetIndicatorActive(false);

            BattleSystem.Instance.start = true;
            spawned = true;
        }
    }
}
