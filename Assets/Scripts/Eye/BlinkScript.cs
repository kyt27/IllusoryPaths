using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class BlinkScript : MonoBehaviour {
    private Camera myCamera;

    void Start () {
        myCamera = Camera.main;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            PostProcessVolume ppVolume = myCamera.gameObject.GetComponent<PostProcessVolume>();
            ppVolume.enabled = !ppVolume.enabled;
        }
    }
}
