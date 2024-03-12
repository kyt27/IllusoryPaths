using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : BaseEyeInteractable {
    public List<Node> nodesToEnable;

    public List<GameObject> objectsToShow;

    public override void Action(Vector3 myInput) {
        Debug.Log("action called");

        foreach(Node node in nodesToEnable) {
            node.EnableNode();
        }

        foreach(GameObject obj in objectsToShow) {
            obj.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
