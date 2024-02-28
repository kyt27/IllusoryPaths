using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDrag : BaseTouch {

    [SerializeField]
    private float rotationSpeed = 100f;
    private bool isRotating = false;
    private float startPosition;

    public GameObject[] rotateTogether;

    [SerializeField]
    private float[] snapAngles = {0, 90, 180, 270};

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
            StopRotation();
        }
        if(isRotating) {
            Rotate(Input.mousePosition.x);
        }
    }

    void HandleDragMobile() {
        if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Ended) {
            StopRotation();
        }
        if(isRotating) {
            Rotate(Input.touches[0].position.x);
        }
    }

    void StopRotation() {
        isRotating = false;
        actionPersist = false;
        
        float angle = (transform.eulerAngles.y + 360) % 360;

        float curMinAngle = 500f;
        int snapTo = -1;

        for(int i=0; i<snapAngles.Length; i++) {
            float temp = System.Math.Min(System.Math.Abs(angle - snapAngles[i]), System.Math.Abs(angle - snapAngles[i] - 360));
            if(temp < curMinAngle) {
                snapTo = i;
                curMinAngle = temp;
            }
        }

        float RotateTo = (snapAngles[snapTo] + 180) % 360 - 180;

        transform.rotation = Quaternion.Euler(0, snapAngles[snapTo], 0);
        foreach(GameObject obj in rotateTogether) {
            obj.transform.rotation = Quaternion.Euler(0, snapAngles[snapTo], 0);
        }
    }

    void Rotate(float currentPosition) {
        float movement = currentPosition - startPosition;
            transform.Rotate(Vector3.up, -movement * rotationSpeed * Time.deltaTime);
            foreach(GameObject obj in rotateTogether) {
                obj.transform.Rotate(Vector3.up, -movement * rotationSpeed * Time.deltaTime);
            }
            startPosition = currentPosition;
    }
}
