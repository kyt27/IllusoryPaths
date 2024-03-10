using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTouch : BaseTouch {
    [SerializeField]
    private List<GameObject> controlled;

    internal override void Action(Vector3 myInput) {
        foreach (GameObject obj in controlled)
        {
            obj.GetComponent<BaseTouch>().Action(myInput);
        }
    }
}
