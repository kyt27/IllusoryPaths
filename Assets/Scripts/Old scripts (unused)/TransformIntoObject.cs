using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformIntoObject : NodeTranslate {

    [SerializeField] private GameObject target;

    private void Start()
    {
        targetPos = target.transform.position;
    }
}
