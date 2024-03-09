using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBlocker : MonoBehaviour{
    
    private Collider collider;

    void Start() {
        collider = GetComponent<Collider>();
    }

    public void Toggle() {
        collider.enabled = !collider.enabled;
    }

    public void Block() {
        collider.enabled = true;
    }

    public void Unblock() {
        collider.enabled = false;
    }
}
