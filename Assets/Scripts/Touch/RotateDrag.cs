using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDrag : BaseTouch {

    [SerializeField]
    private float rotationSpeed = 100f;
    private bool isRotating = false;
    private float startPosition;

    private Vector3 direction;

    public bool xAxis;
    public bool yAxis;
    public bool zAxis;

    public GameObject[] rotateTogether;

    public float[] snapAngles = {0, 90, 180, 270};

    private RotationLinker rotationLinker;

    internal override void Action(Vector3 myInput) {
        actionPersist = true;
        isRotating = true;
        if(yAxis) startPosition = myInput.x;
        else startPosition = myInput.y;
    }

    internal override void Initialise() {
        if(!xAxis && !yAxis && !zAxis) {
            yAxis = true;
        }

        direction = GetDirection();
    }

    public Vector3 GetDirection() {
        if(xAxis) return Vector3.right;
        else if(yAxis) return Vector3.up;
        else if(zAxis) return Vector3.forward;
        else Debug.Log("NO AXIS DETECTED IN ROTATION");
        return new Vector3();
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
            if(yAxis) Rotate(Input.mousePosition.x);
            else Rotate(Input.mousePosition.y);
        }
    }

    void HandleDragMobile() {
        if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Ended) {
            StopRotation();
        }
        if(isRotating) {
            if(yAxis) Rotate(Input.touches[0].position.x);
            else Rotate(Input.touches[0].position.y);
        }
    }
    
    void Rotate(float currentPosition) {
        float movement = currentPosition - startPosition;
        
        transform.Rotate(direction, -movement * rotationSpeed * Time.deltaTime);
        foreach(GameObject obj in rotateTogether) {
            obj.transform.Rotate(direction, -movement * rotationSpeed * Time.deltaTime);
        }
        startPosition = currentPosition;
    }

    public static float GetXDegrees(Transform t) {
        float radians = Mathf.Atan2(t.forward.y, -t.forward.z);
        return 180f + radians * Mathf.Rad2Deg;
    }

    void StopRotation() {
        isRotating = false;
        actionPersist = false;

        float angle = 0f;
        float parentAngle = 0f;
        float rotationAngle = 0f;
        
        if (xAxis) {
            angle = (GetXDegrees(this.transform) + 720) % 360;
            rotationAngle = GetXDegrees(this.transform);
            parentAngle = GetXDegrees(transform.parent.transform);
        } else if (yAxis) {
            angle = (this.transform.rotation.eulerAngles.y + 720) % 360;
            parentAngle = this.transform.parent.transform.rotation.eulerAngles.y;
            rotationAngle = this.transform.rotation.eulerAngles.y;
        } else if (zAxis) {
            angle = (this.transform.rotation.eulerAngles.z + 720) % 360;
            parentAngle = this.transform.parent.transform.rotation.eulerAngles.z;
            rotationAngle = this.transform.rotation.eulerAngles.z;
        }

        float curMinAngle = 1000f;
        int snapTo = -1;

        for(int i=0; i<snapAngles.Length; i++) {
            float temp = System.Math.Min(System.Math.Abs(angle - (snapAngles[i] + parentAngle + 720) % 360), 360 - System.Math.Abs(angle - (snapAngles[i] + parentAngle + 720) % 360));
            if(temp < curMinAngle) {
                snapTo = i;
                curMinAngle = temp;
            }
        }

        float rotateTo = snapAngles[snapTo] + parentAngle;

        foreach(GameObject obj in rotateTogether) {
            obj.transform.Rotate(direction, rotateTo - rotationAngle);
        }
        transform.Rotate(direction, rotateTo - rotationAngle);

        rotationLinker = GetComponent<RotationLinker>();
        if(rotationLinker == null) return;

        rotationLinker.UpdateRotationLinks();
    }
}
