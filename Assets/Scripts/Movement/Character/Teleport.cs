using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodePairs {
    public Node from;
    public Node to;
}

public class Teleport : MonoBehaviour {
    public List<NodePairs> teleportPairs;

    // Update is called once per frame
    public Node findTeleportNode(Node curNode) {
        foreach(NodePairs teleportPair in teleportPairs) {
            if(curNode.name == teleportPair.from.name) return teleportPair.to;
        }
        return null;
    }
}
