using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to activate/deactivate special Edges between Nodes based on shifting
[System.Serializable]
public class ShiftLink {
    // position needed to activate link
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
        Debug.Log("updated shift links");
        foreach (ShiftLink l in shiftLinks) {
            if (l.pos == null || l.nodeA == null || l.nodeB == null) continue;

            // check difference between desired and current position
            float posDiff = Vector3.Distance(transform.localPosition, l.pos);

            Debug.Log(transform.localPosition);

            // enable the linked Edges if the position matches; otherwise disable
            if (Mathf.Abs(posDiff) < 0.01f) {
                EnableLink(l.nodeA, l.nodeB, true);
            } else {
                EnableLink(l.nodeA, l.nodeB, false);
            }
        }
    }

    public void ActivateAllShiftLinks()
    {
        foreach (ShiftLink l in shiftLinks)
        {
            EnableLink(l.nodeA, l.nodeB, true);
        }
    }

    // update links when we begin
    private void Start() {
        UpdateShiftLinks();
    }
}
