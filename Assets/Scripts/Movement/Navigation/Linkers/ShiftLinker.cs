using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to activate/deactivate special Edges between Nodes based on rotation
[System.Serializable]
public class ShiftLink {
    // euler angle needed to activate link
    public Vector3 pos;
    [Header("Nodes to activate")]
    public Node nodeA;
    public Node nodeB;
}

public class ShiftLinker : MonoBehaviour {
    [SerializeField] public ShiftLink[] shiftLinks;

    // toggle active state of Edge between neighbor Nodes
    public void EnableLink(Node nodeA, Node nodeB, bool state) {
        if (nodeA == null || nodeB == null) return;

        nodeA.EnableEdge(nodeB, state);
        nodeB.EnableEdge(nodeA, state);
    }

    // enable/disable based on transform's position
    public void UpdateShiftLinks() {
        foreach (ShiftLink l in shiftLinks) {
            if (l.pos == null || l.nodeA == null || l.nodeB == null) continue;

            // check difference between desired and current angle
            float posDiff = Vector3.Distance(transform.position, l.pos);

            // enable the linked Edges if the position matches; otherwise disable
            if (Mathf.Abs(posDiff) < 0.01f) {
                EnableLink(l.nodeA, l.nodeB, true);
            } else {
                EnableLink(l.nodeA, l.nodeB, false);
            }
        }
    }

    // update links when we begin
    private void Start() {
        UpdateShiftLinks();
    }
}
