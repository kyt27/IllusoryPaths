using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRise : BaseTouch {
    internal override void Action(Vector3 myInput) {
        transform.position += Vector3.up;
    }
}
