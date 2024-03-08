using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTouch : MonoBehaviour {
    [SerializeField] 
    private List<Material> eyeMaterials;

    private bool interactable = false;

    [SerializeField] private float hintViewAngle = 30.0f;
    [SerializeField] private float hintViewDist = 5.0f;

    [SerializeField] private float acceptViewAngle = 20.0f;
    [SerializeField] private float acceptViewDist = 3.5f;

    [SerializeField] private GameObject controlled;

    private Camera myCamera;
    private Vector3 normal;
    private Ray ray;
    private RaycastHit hit;

    [SerializeField] private bool testInteractable = false;

    void Start() {
        myCamera = Camera.main;

        if(hintViewAngle < acceptViewAngle) throw new System.Exception("hintViewAngle must be greater or equal to acceptViewAngle");
        if(hintViewDist < acceptViewDist) throw new System.Exception("hintViewDist must be greater or equal to acceptViewDist");

        if(controlled == null) throw new System.Exception("no object assigned to eye");
        if(controlled.GetComponent<BaseEyeInteractable>() == null) throw new System.Exception("assigned object needs to contain script extending BaseEyeInteractable");
    }

    void Update() {
        Vector3 eyeNormal = transform.forward;
        Vector3 eyePosition = transform.position;

        Vector3 viewVector = myCamera.transform.forward;
        Vector3 viewPosition = myCamera.transform.position;

        float k = (Vector3.Dot(eyeNormal, eyePosition) - Vector3.Dot(eyeNormal, viewPosition)) / Vector3.Dot(eyeNormal, viewVector);
        Vector3 intersectPoint = viewPosition + viewVector * k;

        float viewAngle = Vector3.Angle(eyeNormal, viewVector);
        float viewDist = Vector3.Distance(intersectPoint, eyePosition);

        if(testInteractable) {
            GetComponent<MeshRenderer>().material = eyeMaterials[2];
        } else if(viewAngle > hintViewAngle || viewDist > hintViewDist) {
            GetComponent<MeshRenderer>().material = eyeMaterials[0];
            interactable = false;
        } else if(viewAngle > acceptViewAngle || viewDist > acceptViewDist) {
            GetComponent<MeshRenderer>().material = eyeMaterials[1];
            interactable = false;
        } else {
            GetComponent<MeshRenderer>().material = eyeMaterials[2];
            interactable = true;
        }

        if(interactable || testInteractable) {
            IsClickObject();
        }
    }

    void HandleRayCast(Vector3 myInput) {
        ray = myCamera.ScreenPointToRay(myInput);
        if(Physics.Raycast(ray, out hit)) {
            if(GameObject.ReferenceEquals(hit.transform.gameObject, this.gameObject)) {
                // Debug.Log(this.gameObject.name + myInput);
                transform.parent.gameObject.GetComponentsInChildren<NavController>()[0].GetComponent<NavController>().SnapToCurrentNode();
                controlled.GetComponent<BaseEyeInteractable>().Action(myInput);
            }
        }
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
