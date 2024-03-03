using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeChanger : MonoBehaviour {

    /*
    [SerializeField] 
    private List<Material> eyeMaterials;

    [SerializeField] private float hintViewAngle = 30.0f;
    [SerializeField] private float hintViewDist = 5.0f;

    [SerializeField] private float acceptViewAngle = 20.0f;
    [SerializeField] private float acceptViewDist = 3.5f;

    private Camera myCamera;
    private Vector3 normal;

    void Start() {
        myCamera = Camera.main;

        if(hintViewAngle < acceptViewAngle) throw new System.Exception("hintViewAngle must be greater or equal to acceptViewAngle");
        if(hintViewDist < acceptViewDist) throw new System.Exception("hintViewDist must be greater or equal to acceptViewDist");
    }

    void Update() {
        Vector3 eyeNormal = transform.forward;
        Vector3 eyePosition = transform.position;

        Vector3 viewVector = myCamera.transform.forward;
        Vector3 viewPosition = myCamera.transform.position;

        float k = (Vector3.Dot(eyeNormal, eyePosition) - Vector3.Dot(eyeNormal, viewPosition)) / Vector3.Dot(eyeNormal, viewVector);;
        Vector3 intersectPoint = viewPosition + viewVector * k;

        float viewAngle = Vector3.Angle(eyeNormal, viewVector);
        float viewDist = Vector3.Distance(intersectPoint, eyePosition);

        if(viewAngle > hintViewAngle || viewDist > hintViewDist) {
            GetComponent<MeshRenderer>().material = eyeMaterials[0];
        } else if(viewAngle > acceptViewAngle || viewDist > acceptViewDist) {
            GetComponent<MeshRenderer>().material = eyeMaterials[1];
        } else {
            GetComponent<MeshRenderer>().material = eyeMaterials[2];
        }
    }

    */
}
