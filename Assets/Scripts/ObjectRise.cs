using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRise : BaseTouch {
    internal override void Action() {
        transform.position += Vector3.up;
    }
}
