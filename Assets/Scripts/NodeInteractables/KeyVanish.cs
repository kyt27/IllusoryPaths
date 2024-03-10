using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyVanish : BaseNodeInteractable {
    public Node node;

    bool activated = false;

    public override void Action(Vector3 myInput) {
        if(!activated && node != null) {
            node.EnableNode();
            Destroy(gameObject);
            activated = true;
        }
    }
}
