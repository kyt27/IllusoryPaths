using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTouch : MonoBehaviour {
    private InputManager inputManager;
    private Camera cameraMain;

    private void Awake() {
        inputManager = InputManager.Instance;
        cameraMain = Camera.main;
    }

    private void OnEnable() {
        inputManager.OnStartTouch += ChangeColor;
    }

    private void OnDisable() {
        inputManager.OnEndTouch -= ChangeColor;
    }

    public void ChangeColor(Vector2 screenPosition, float time) {
        Vector3 screenCoords = new Vector3(screenPosition.x, screenPosition.y, cameraMain.nearClipPlane);
        Vector3 worldCoords = cameraMain.ScreenToWorldPoint(screenCoordinates);
        transform.position = worldCoords;
    }
}
