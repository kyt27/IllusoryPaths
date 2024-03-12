using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTouch : MonoBehaviour {
    private Camera myCamera;
    private Ray ray;
    private RaycastHit hit;

    internal bool actionPersist = false;

    void Start() {
        myCamera = Camera.main;
        Initialise();
    }

    void Update() {
        IsClickObject();
        if(actionPersist) {
            TogglePersist();
            transform.parent.gameObject.GetComponentsInChildren<NavController>()[0].GetComponent<NavController>().SnapToCurrentNode();
        }
    }

    void HandleRayCast(Vector3 myInput) {
        ray = myCamera.ScreenPointToRay(myInput);
        if(Physics.Raycast(ray, out hit)) {
            if(GameObject.ReferenceEquals(hit.transform.gameObject, this.gameObject)) {
                // Debug.Log(this.gameObject.name + myInput);
                transform.parent.gameObject.GetComponentsInChildren<NavController>()[0].GetComponent<NavController>().SnapToCurrentNode();
                Action(myInput);
            }
        }
    }

    internal virtual void Initialise() {
        return;
    }

    internal abstract void Action(Vector3 myInput);

    internal virtual void TogglePersist() {
        return;
    }

    void HandleClickMouse() {
        if (Input.GetMouseButtonDown(0)) {
            HandleRayCast(Input.mousePosition);
        }
    }

    void HandleTouchMobile() {
        if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began) {
            HandleRayCast(Input.touches[0].position);
        }
    }

    void IsClickObject() {
        # if UNITY_EDITOR || UNITY_STANDALONE
        HandleClickMouse();
        # elif UNITY_ANDROID || UNITY_IOS
        HandleTouchMobile();
        # endif
    }
}
