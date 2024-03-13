using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardCheck : MonoBehaviour {
    public GameObject level;
    public float shiftBy;

    private bool atA;

    // Start is called before the first frame update
    void Awake() {
        if(Vector3.Dot(transform.position - Camera.main.transform.position, transform.forward) > 0) {
            atA = true;
        } else {
            atA = false;
        }

        Debug.Log(atA);
    }

    // Update is called once per frame
    void Update() {
        if(atA) {
            if(Vector3.Dot(transform.position - Camera.main.transform.position, transform.forward) < 0) {
                atA = false;
                level.transform.localPosition += Vector3.left * shiftBy;
            }
        } else {
            if(Vector3.Dot(transform.position - Camera.main.transform.position, transform.forward) > 0) {
                atA = true;
                level.transform.localPosition += Vector3.right * shiftBy;
            }
        }
    }
}
