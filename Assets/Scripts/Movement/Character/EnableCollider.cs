using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCollider : MonoBehaviour {
    Collider colldier;
    void Start() {
        colldier = GetComponent<Collider>();
        colldier.enabled = true;
    }
}
