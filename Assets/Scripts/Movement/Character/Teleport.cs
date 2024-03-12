using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
    List<Tuple<Node, Node>> teleportPairs;

    // Update is called once per frame
    public Node findTeleportNode(Node curNode) {
        foreach(Tuple<Node, Node> teleportPair in teleportPairs) {
            if(curNode.name == teleportPair.Item1.name) return teleportPair.Item2;
        }
        return null;
    }
}
