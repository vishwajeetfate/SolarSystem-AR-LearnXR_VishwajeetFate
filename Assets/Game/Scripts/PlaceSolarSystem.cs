using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceSolarSystem : MonoBehaviour
{
    public GameObject solarSystemPrefab;
    private GameObject placedObject;
    private ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (InteractionManager.IsFrozen) return;
        if (placedObject != null) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                placedObject = Instantiate(solarSystemPrefab, hitPose.position, hitPose.rotation);
                Debug.Log("Solar system placed.");
            }
        }
    }
}
