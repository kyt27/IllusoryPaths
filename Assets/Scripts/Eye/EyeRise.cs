using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRise : BaseEyeInteractable {
    public override void Action(Vector3 myInput) {
        transform.position += Vector3.up;
    }
}
