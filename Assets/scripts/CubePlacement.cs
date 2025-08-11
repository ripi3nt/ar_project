using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CubePlacement : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public static GameObject entity;
    public ARRaycastManager raycastManager;
    public static bool placed = false;

    void OnEnable()
    {
        planeManager.planesChanged += handleDetectedPlanes;
    }

    void handleDetectedPlanes(ARPlanesChangedEventArgs args)
    {
        Debug.Log("Looking for planes...");

        if (placed) return;
        if (entity == null ) return;

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            //Debug.Log("Plane hit! Placing pet at: " + hitPose.position);
    
            entity.transform.position = hitPose.position;
            Vector3 direction = Camera.main.transform.position - hitPose.position;
            direction.y = 0;
            entity.transform.rotation = Quaternion.LookRotation(direction);

            entity.SetActive(true);
            Debug.Log("Pet activated!");

            placed = true;
        }
    }


    void OnDisable()
    {
        planeManager.planesChanged -= handleDetectedPlanes;    
    }
}
