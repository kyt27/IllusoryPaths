using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRise : TouchTranslate {

    [SerializeField] private float riseDistance = 1;

    internal override void UpdateTarget() {
        targetPos = transform.position + new Vector3(0, riseDistance, 0);
    }
}
