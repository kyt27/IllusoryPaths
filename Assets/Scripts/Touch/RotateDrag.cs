using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDrag : BaseTouch {

    [SerializeField]
    private float rotationSpeed = 100f;
    private bool isRotating = false;
    private float startPosition;

    internal override void Action(Vector3 myInput) {
        actionPersist = true;
        isRotating = true;
        startPosition = myInput.x;
    }

    internal override void TogglePersist() {
        # if UNITY_EDITOR || UNITY_STANDALONE
        HandleDragMouse();
        # elif UNITY_ANDROID || UNITY_IOS
        HandleDragMobile();
        # endif
    }

    void HandleDragMouse() {
        if(Input.GetMouseButtonUp(0)) {
            Debug.Log("Mouse Up");
            isRotating = false;
            actionPersist = false;
        }

        if(isRotating) {
            float currentPosition = Input.mousePosition.x;
            float movement = currentPosition - startPosition;

            transform.Rotate(Vector3.up, -movement * rotationSpeed * Time.deltaTime);
            startPosition = currentPosition;
        }
    }

    void HandleDragMobile() {
        if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Ended) {
            isRotating = false;
            actionPersist = false;
        }

        if(isRotating) {
            float currentPosition = Input.touches[0].position.x;
            float movement = currentPosition - startPosition;

            transform.Rotate(Vector3.up, -movement * rotationSpeed * Time.deltaTime);
            startPosition = currentPosition;
        }
    }
}
