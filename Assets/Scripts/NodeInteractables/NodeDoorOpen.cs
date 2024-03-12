using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDoorOpen : CountEnable {
    public Node nodeToActivate;

    public GameObject doorToShift;

    internal override void Action() {
        nodeToActivate.EnableNode();
        doorToShift.transform.localPosition += Vector3.down;
    }
}
