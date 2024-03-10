using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTranslate : BaseEyeInteractable {
    [SerializeField] private Vector3 pos;

    public override void Action(Vector3 myInput) {
        transform.localPosition = pos;
    }
}
