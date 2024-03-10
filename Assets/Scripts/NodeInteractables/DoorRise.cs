using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRise : BaseNodeInteractable {
    public Node node;

    [SerializeField] private int riseDistance = 2;

    bool activated = false;

    private ShiftLinker shiftLinker;

    private void Start()
    {
        shiftLinker = GetComponent<ShiftLinker>();
    }

    public override void Action(Vector3 myInput) {
        if(!activated && node != null) {
            node.EnableNode();
            transform.position += new Vector3(0, riseDistance, 0);
            activated = true;
            shiftLinker.UpdateShiftLinks();
        }
    }
}
