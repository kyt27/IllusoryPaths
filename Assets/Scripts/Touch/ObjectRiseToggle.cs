using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRiseToggle : TouchTranslate {

    [Header("Set negative distance to move down first")]
    [SerializeField] private float distanceToMove = 1;
    private int direction = 1;

    internal override void UpdateTarget() {
        targetPos = transform.position + direction * new Vector3(0, distanceToMove, 0);

        // flip direction
        direction *= -1;
    }
}
