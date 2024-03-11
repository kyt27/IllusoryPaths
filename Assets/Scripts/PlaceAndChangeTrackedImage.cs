using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlaceAndChangeTrackedImage : MonoBehaviour {
    // Reference to AR tracked image manager component
    private ARTrackedImageManager _trackedImageManager;

    // List of prefabs to instantiate - these should be named the same
    // as their corresponding 2D images in the reference image library

    [SerializeField]
    private GameObject[] ArPrefabs;
    private static int sceneLength = 0;

    [SerializeField]
    private static int sceneNumber = 0;
    public static GameObject[] _instantiatedPrefabs;

    [SerializeField]
    private GameObject testInstantiate;

    private string curPrefab;

    void Awake() {
        // Cache a reference to the Tracked Image Manager component
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        sceneLength = ArPrefabs.Length;
        _instantiatedPrefabs = new GameObject[sceneLength];
        curPrefab = "";
    }

    void OnEnable() {
        // Attach event handler when tracked images change
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
        // Loop through all prefabs, instantiate it and parent it to the ARTrackedImage
        for(int i=0; i<sceneLength; i++) {
            _instantiatedPrefabs[i] = Instantiate(ArPrefabs[i], eventArgs.added[0].transform);
            if(i != sceneNumber) {
                _instantiatedPrefabs[i].SetActive(false);
            }
        }

        Instantiate(testInstantiate, _instantiatedPrefabs[0].transform);
    
        // For all prefabs that have been created so far, set them active or not depending on whether their corresponding image is currently being tracked
        foreach (var trackedImage in eventArgs.updated) {
            _instantiatedPrefabs[sceneNumber].SetActive(true);
        }

        foreach (var trackedImage in eventArgs.removed) {
            
        }

    }

    public static void ChangePrefab() {
        Debug.Log(sceneLength + "," + sceneNumber);

        _instantiatedPrefabs[sceneNumber].SetActive(false);
        sceneNumber = (sceneNumber + 1) % sceneLength;
        _instantiatedPrefabs[sceneNumber].SetActive(true);
    }
}
