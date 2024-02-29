using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeChanger : MonoBehaviour
{

    [SerializeField] 
    private List<Material> eyeMaterials;

    [SerializeField] 
    private float acceptViewAngle = 30.0f;

    [SerializeField] 
    private float acceptViewDist = 10.0f;

    private Camera myCamera;
    private Vector3 normal;

    void Start() {
        myCamera = Camera.main;
    }

    void Update() {
        if(false) {
            Vector3 eyeNormal = transform.forward;
            Vector3 eyePosition = transform.position;

            Vector3 viewVector = myCamera.transform.forward;
            Vector3 viewPosition = myCamera.transform.position;

            if(Vector3.Angle(eyeNormal, viewVector) > acceptViewAngle) {
                GetComponent<MeshRenderer>().material = eyeMaterials[0];
            } else {
                float k = (Vector3.Dot(eyeNormal, eyePosition) - Vector3.Dot(eyeNormal, viewPosition)) / Vector3.Dot(eyeNormal, viewVector);;
                Vector3 intersectPoint = viewPosition + viewVector * k;

                if(Vector3.Distance(intersectPoint, eyePosition) > acceptViewDist) {
                    GetComponent<MeshRenderer>().material = eyeMaterials[1];
                } else {
                    GetComponent<MeshRenderer>().material = eyeMaterials[2];
                }
            }
        }
    }
}
