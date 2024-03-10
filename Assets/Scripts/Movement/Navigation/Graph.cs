using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // all of the Nodes in the current level/maze
    private List<Node> allNodes = new List<Node>();

    // end of level
    [SerializeField] private Node goalNode;
    public Node GoalNode => goalNode;

    private void Awake() {
        GraphHelper helper = GetComponent<GraphHelper>();
        if(helper != null) {
            allNodes = helper.allNodes;
        } else {
            allNodes = GetComponentsInChildren<Node>().ToList();
        }
        // Debug.Log(allNodes.Count);
    }

    private void Start() {
        InitNodes();
        InitNeighbours();
    }

    // locate the specific Node at target position within rounding error
    public Node FindNodeAt(Vector3 pos) {
        foreach (Node n in allNodes) {
            Vector3 diff = n.transform.position - pos;

            if (diff.sqrMagnitude < 0.01f) return n;
        }
        return null;
    }

    // locate the closest Node in screen space, given an array of Nodes
    public Node FindClosestNode(Node[] nodes, Vector3 pos) {
        Node closestNode = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Node n in nodes) {
            Vector3 diff = n.transform.position - pos;

            // Vector3 nodeScreenPosition = Camera.main.WorldToScreenPoint(n.transform.position);
            // Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos);
            // diff = nodeScreenPosition - screenPosition;

            if (diff.sqrMagnitude < closestDistanceSqr) {
                closestNode = n;
                closestDistanceSqr = diff.sqrMagnitude;
            }
        }

        // Debug.Log(closestDistanceSqr);
        return closestNode;
    }

    // find the closest Node in the entire Graph
    public Node FindClosestNode(Vector3 pos) {
        return FindClosestNode(allNodes.ToArray(), pos);
    }

    public Node[] FindClosestNodes(Vector3 pos, float distance = 0.75f) {
        List<Node> nodes = new List<Node>();
        List<float> dist = new List<float>();

        foreach (Node n in allNodes) {
            Vector3 diff = n.transform.position - pos;

            // Vector3 nodeScreenPosition = Camera.main.WorldToScreenPoint(n.transform.position);
            // Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos);
            // diff = nodeScreenPosition - screenPosition;

            if (diff.sqrMagnitude < distance) {
                nodes.Add(n);
                dist.Add(diff.sqrMagnitude);
            }
        }

        Node[] nodesArray = nodes.ToArray();
        float[] distArray = dist.ToArray();
        
        if(nodesArray.Length > 0) {
            System.Array.Sort(distArray, nodesArray);
        }

        return nodesArray;
    }

    // clear breadcrumb trail
    public void ResetNodes() {
        foreach (Node node in allNodes) {
            node.PreviousNode = null;
        }
    }

    // set the Graph for each Node
    private void InitNodes() {
        foreach (Node n in allNodes) {
            if (n != null) {
                n.InitGraph(this);
            }
        }
    }

    // set neighbors for each Node; must run AFTER all Nodes are initialized
    private void InitNeighbours() {
        foreach (Node n in allNodes) {
            if (n != null) {
                n.FindNeighbours();
            }
        }
    }
}
