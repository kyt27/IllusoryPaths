using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchObject : MonoBehaviour {
    private Camera myCamera;
    private Ray ray;
    private RaycastHit hit;

    private enum ManageClick {
        NONE,
        DOWN,
        HELD,
        UP,
    }

    [SerializeField]
    private ManageClick myManageClick;

    void Start() {
        myCamera = Camera.main;
    }

    void Update() {
        IsClickObject();
    }

    void HandleRayCast(Vector3 myInput) {
        Debug.Log(myInput);
        ray = myCamera.ScreenPointToRay(myInput);
        if(Physics.Raycast(ray, out hit)) {
            switch(hit.transform.gameObject.name) {
                case "OrangeCube":
                    hit.transform.position += Vector3.up;
                    break;
                case "GreenCube":
                    hit.transform.position += Vector3.up;
                    break;
            }
        }
    }

    void HandleClickMouse() {
        switch (myManageClick) {
            case ManageClick.NONE:
                break;
            case ManageClick.DOWN:
                if (Input.GetMouseButtonDown(0)) {
                    HandleRayCast(Input.mousePosition);
                }
                break;
            case ManageClick.HELD:
                if (Input.GetMouseButton(0)) {
                    HandleRayCast(Input.mousePosition);
                }
                break;
            case ManageClick.UP:
                if (Input.GetMouseButtonUp(0)) {
                    HandleRayCast(Input.mousePosition);
                }
                break;
            default:
                break;
        }
    }

    void HandleTouchMobile() {
        switch (myManageClick) {
            case ManageClick.NONE:
                break;
            case ManageClick.DOWN:
                if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began) {
                    HandleRayCast(Input.touches[0].position);
                }
                break;
            case ManageClick.HELD:
                if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Moved) {
                    HandleRayCast(Input.touches[0].position);
                }
                break;
            case ManageClick.UP:
                if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Ended) {
                    HandleRayCast(Input.touches[0].position);
                }
                break;
            default:
                break;
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
