using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to activate/deactivate special Edges between Nodes based on rotation
[System.Serializable]
public class RotationLink {
    // euler angle needed to activate link
    public Vector3 activeEulerAngle;
    [Header("Nodes to activate")]
    public Node nodeA;
    public Node nodeB;
}

[RequireComponent(typeof(RotateDrag))]
public class RotationLinker : MonoBehaviour {
    [SerializeField] public RotationLink[] rotationLinks;

    // toggle active state of Edge between neighbor Nodes
    public void EnableLink(Node nodeA, Node nodeB, bool state) {
        if (nodeA == null || nodeB == null) return;

        nodeA.EnableEdge(nodeB, state);
        nodeB.EnableEdge(nodeA, state);
    }

    // enable/disable based on transform's euler angles
    public void UpdateRotationLinks() {
        foreach (RotationLink l in rotationLinks) {
            if (transform == null || l.nodeA == null || l.nodeB == null) continue;

            // check difference between desired and current angle
            Quaternion targetAngle = Quaternion.Euler(l.activeEulerAngle);
            float angleDiff = Quaternion.Angle(transform.rotation, targetAngle);

            // enable the linked Edges if the angle matches; otherwise disable
            if (Mathf.Abs(angleDiff) < 0.01f) {
                EnableLink(l.nodeA, l.nodeB, true);
            } else {
                EnableLink(l.nodeA, l.nodeB, false);
            }
        }
    }

    // update links when we begin
    private void Start() {
        UpdateRotationLinks();
    }
}
