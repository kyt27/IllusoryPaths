using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphHelper : MonoBehaviour {
    // Attach to game area
    // all of the Nodes in the current level/maze
    public List<Node> allNodes = new List<Node>();

    private void Awake() {
        allNodes = GetComponentsInChildren<Node>().ToList();
    }
}
