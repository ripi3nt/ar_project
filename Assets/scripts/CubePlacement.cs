using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CubePlacement : MonoBehaviour
{
    public ARPlaneManager planeManager;
    private GameObject entity;
    public ARRaycastManager raycastManager;

    public static bool placed = false;

    void Start()
    {
        EntityManager.Instance.SpawnPet();
        entity = EntityManager.Instance.PetInstance;
    }

    void OnEnable()
    {
        planeManager.planesChanged += handleDetectedPlanes;    
    }

    void handleDetectedPlanes(ARPlanesChangedEventArgs args)
    {
        /*if (placed || args.added.Count == 0) return;

        ARPlane firstPlane = args.added[0];
        entity.transform.position = firstPlane.center;
        entity.SetActive(true);
        placed = true;
        */
       if (placed) return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        // Raycast from screen center into the real world, hit any detected plane
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            entity.transform.position = hitPose.position;
            //entity.transform.rotation = hitPose.rotation;
            Vector3 direction = Camera.main.transform.position - hitPose.position;
            direction.y = 0; 
            entity.transform.rotation = Quaternion.LookRotation(direction);

            entity.SetActive(true);
            placed = true;
        } 
    }

    void OnDisable()
    {
        planeManager.planesChanged -= handleDetectedPlanes;    
    }
}
