using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class NewPlaceAndTrack : MonoBehaviour {
    private ARTrackedImageManager _trackedImageManager;
    
    [SerializeField] private GameObject objectHolder;
    private static GameObject _instantiatedHolder;

    [SerializeField] private GameObject mainMenuScene;
    private static GameObject _instantiatedMainMenu;

    [SerializeField] private GameObject levelSelectScene;
    private static GameObject _instantiatedLevelSelect;

    [SerializeField] private List<GameObject> levelScenes;
    private static bool levelInstantiated;
    private static GameObject _instantiatedLevel;

    void Awake() {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        levelInstantiated = false;
    }

    void OnEnable() {
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
        _instantiatedHolder = Instantiate(objectHolder, eventArgs.added[0].transform);
        _instantiatedMainMenu = Instantiate(mainMenuScene, _instantiatedHolder.transform);
        _instantiatedLevelSelect = Instantiate(levelSelectScene, _instantiatedHolder.transform);

        _instantiatedHolder.SetActive(true);
        _instantiatedMainMenu.SetActive(true);
        _instantiatedLevelSelect.SetActive(false);
    
        foreach (var trackedImage in eventArgs.updated) {

        }

        foreach (var trackedImage in eventArgs.removed) {
            
        }

    }

    public void ChangeScene(string levelName) {
        switch(levelName) {
            case "mainMenu":
                _instantiatedLevelSelect.SetActive(false);
                if(levelInstantiated) {
                    levelInstantiated = false;
                    Destroy(_instantiatedLevel);
                }
                _instantiatedMainMenu.SetActive(true);
                break;
            case "levelSelect":
                _instantiatedMainMenu.SetActive(false);
                if(levelInstantiated) {
                    levelInstantiated = false;
                    Destroy(_instantiatedLevel);
                }
                _instantiatedLevelSelect.SetActive(true);
                break;
            default:
                _instantiatedMainMenu.SetActive(false);
                _instantiatedLevelSelect.SetActive(false);
                if(levelInstantiated) {
                    levelInstantiated = false;
                    Destroy(_instantiatedLevel);
                }
                bool foundMatchingObject = false;
                foreach(GameObject level in levelScenes) {
                    if(string.Compare(level.name, levelName, System.StringComparison.OrdinalIgnoreCase) == 0) {
                        foundMatchingObject = true;
                        _instantiatedLevel = Instantiate(level, _instantiatedHolder.transform);
                        levelInstantiated = true;
                        break;
                    }
                }
                if(!foundMatchingObject) _instantiatedMainMenu.SetActive(true);
                break;
        }
    }
}

