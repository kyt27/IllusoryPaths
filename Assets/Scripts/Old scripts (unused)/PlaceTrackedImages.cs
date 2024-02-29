using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PlaceTrackedImages : MonoBehaviour {
    // Reference to AR tracked image manager component
    private ARTrackedImageManager _trackedImageManager;

    // List of prefabs to instantiate - these should be named the same
    // as their corresponding 2D images in the reference image library
    public GameObject[] ArPrefabs;

    [SerializeField]
    private int sceneNumber = 0;

    private readonly Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();

    void Awake() {
        // Cache a reference to the Tracked Image Manager component
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            sceneNumber = (sceneNumber + 1) % ArPrefabs.Length;
        }
    }

    void OnEnable() {
        // Attach event handler when tracked images change
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable() {
        // Remove event handler
        // _trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    // Event Handler
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
        // Loop through all new tracked images that have been detected
        foreach (var trackedImage in eventArgs.added) {
            // Get the name of the reference image
            var imageName = trackedImage.referenceImage.name;
            foreach(var curPrefab in ArPrefabs) {
                // Check whether this prefab matches the tracked image name, and that the prefab hasn't already been created
                if (string.Compare(curPrefab.name, imageName, System.StringComparison.OrdinalIgnoreCase) == 0 && !_instantiatedPrefabs.ContainsKey(imageName)) {
                    // Instantiate the prefab, parenting it to the ARTrackedImage
                    var newPrefab = Instantiate(curPrefab, trackedImage.transform);
                    // Add the created prefab to our array
                    _instantiatedPrefabs[imageName] = newPrefab;
                }
            }
        }

        // For all prefabs that have been created so far, set them active or not depending on whether their corresponding image is currently being tracked
        foreach (var trackedImage in eventArgs.updated) {
            _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(true);
        }

        // var newPrefab = Instantiate(ArPrefabs[0], eventArgs.added[0].transform);
    }
}
