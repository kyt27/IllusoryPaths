using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTouch : MonoBehaviour {
    [SerializeField] 
    private List<Material> eyeMaterials;

    private bool interactable = false;

    public bool enabled = true;

    [SerializeField] private float hintViewAngle = 30.0f;
    [SerializeField] private float hintViewDist = 5.0f;

    [SerializeField] private float acceptViewAngle = 20.0f;
    [SerializeField] private float acceptViewDist = 3.5f;

    [SerializeField] private GameObject controlledObjects;

    private Camera myCamera;
    private Vector3 normal;
    private Ray ray;
    private RaycastHit hit;

    [SerializeField] private bool testInteractable = false;

    private BlinkManager blinkManager;

    public bool triggerBlinkAnimation = true;
    public BaseEyeInteractable eyeGlow;

    void Start() {
        myCamera = Camera.main;

        if(hintViewAngle < acceptViewAngle) Debug.Log("hintViewAngle must be greater or equal to acceptViewAngle");
        if(hintViewDist < acceptViewDist) Debug.Log("hintViewDist must be greater or equal to acceptViewDist");

        if(controlledObjects == null) Debug.Log("no object assigned to eye");
        if(controlledObjects.GetComponent<BaseEyeInteractable>() == null) Debug.Log("assigned object needs to contain script extending BaseEyeInteractable");

        BlinkManager[] temp = FindObjectsOfType<BlinkManager>();
        if(temp.Length == 0) Debug.Log("blink manager not detected");
        else if(temp.Length > 1) Debug.Log("too many blink managers");
        else if(temp.Length == 1) blinkManager = temp[0];

        GetComponent<MeshRenderer>().material = eyeMaterials[0];
        if(eyeGlow == null) eyeGlow = GetComponent<EyeGlow>();
    }

    void Update() {
        if(enabled) {
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
                interactable = true;
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

            if(interactable) {
                IsClickObject();
            }
        }
    }

    void HandleRayCast(Vector3 myInput) {
        ray = myCamera.ScreenPointToRay(myInput);
        if(Physics.Raycast(ray, out hit)) {
            if(GameObject.ReferenceEquals(hit.transform.gameObject, this.gameObject)) {
                transform.parent.gameObject.transform.parent.gameObject.GetComponentsInChildren<NavController>()[0].GetComponent<NavController>().SnapToCurrentNode();
                if(triggerBlinkAnimation && blinkManager != null) blinkManager.Blink();
                controlledObjects.GetComponent<BaseEyeInteractable>().Action(myInput);
                if(eyeGlow != null) eyeGlow.Action(myInput);
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
