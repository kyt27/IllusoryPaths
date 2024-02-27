using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTouch : BaseTouch {
    [SerializeField]
    private GameObject controlled;

    internal override void Action(Vector3 myInput) {
        controlled.transform.position += Vector3.up;
    }
}
