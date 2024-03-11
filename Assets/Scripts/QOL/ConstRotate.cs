using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstRotate : MonoBehaviour {
    [SerializeField] private bool xAxis;
    [SerializeField] private bool yAxis;
    [SerializeField] private bool zAxis;

    [SerializeField] private float rotationSpeed = 50f;

    void Start() {
        if(!xAxis && !yAxis && !zAxis) {
            Debug.Log("Warning: No ConstRotate direction set for " + transform.name + " at " + transform.position + " , default set to y-axis");
            yAxis = true;
        }
    }

    // Update is called once per frame
    void Update() {
        if(xAxis) {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        } else if (yAxis) {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        } else if (zAxis) {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }
}
