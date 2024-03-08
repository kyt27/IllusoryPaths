using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRise : BaseNodeInteractable {
    public Node node;

    bool activated = false;

    public override void Action(Vector3 myInput) {
        if(!activated && node != null) {
            node.EnableNode();
            transform.position += new Vector3(0, 2, 0);
            activated = true;
        }
    }
}
