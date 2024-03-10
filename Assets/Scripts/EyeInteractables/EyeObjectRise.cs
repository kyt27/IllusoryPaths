using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeObjectRise : EyeTranslate {

    [SerializeField] private float riseDistance = 1;

    internal override void UpdateTarget()
    {
        targetPos = transform.position + new Vector3(0, riseDistance, 0);
    }

}