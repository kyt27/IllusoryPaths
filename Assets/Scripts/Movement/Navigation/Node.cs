using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    // gizmo colors
    [SerializeField] private float gizmoRadius = 0.1f;
    [SerializeField] private Color defaultGizmoColor = Color.black;
    [SerializeField] private Color selectedGizmoColor = Color.blue;
    [SerializeField] private Color inactiveGizmoColor = Color.gray;

    // neighboring nodes + active state
    [SerializeField] private List<Edge> edges = new List<Edge>();

    // Nodes specifically excluded from Edges
    [SerializeField] private List<Node> excludedNodes;

    // reference to the graph
    private Graph graph;

    // previous Node that forms a "breadcrumb" trail back to the start
    private Node previousNode;

    // invoked when Player enters this node
    public GameObject gameEvent;

    // properties
    
    public Node PreviousNode { get { return previousNode; } set { previousNode = value; } }
    public List<Edge> Edges => edges;

    // 3d compass directions to check for all neighbors automatically
    public static Vector3[] neighbourDirections = {
        new Vector3(1f, 0f, 0f),
        new Vector3(-1f, 0f, 0f),
        new Vector3(0f, 0f, 1f),
        new Vector3(0f, 0f, -1f),
    };
        
    private void Start() {
        // automatic connect Edges with horizontal Nodes
        if (graph != null) {
            FindNeighbours();
        }
    }

    // draws a sphere gizmo
    private void OnDrawGizmos() {
        Gizmos.color = defaultGizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }

    // draws a sphere gizmo in a different color when selected
    private void OnDrawGizmosSelected() {
        Gizmos.color = selectedGizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);

        // draws a line to each neighbour
        foreach (Edge e in edges) {
            if (e.neighbour != null) {
                Gizmos.color = e.isActive ? selectedGizmoColor : inactiveGizmoColor;
                Gizmos.DrawLine(transform.position, e.neighbour.transform.position);
            }
        }
    }

    // fill out edge connections to neighbouring nodes automatically
    public void FindNeighbours() {
        // search through possible neighbour offsets
        foreach (Vector3 direction in neighbourDirections) {
            Node newNode = graph?.FindNodeAt(transform.position + direction);

            // add to edges list if not already included and not excluded specifically
            if (newNode != null && !HasNeighbour(newNode) && !excludedNodes.Contains(newNode)) {
                Edge newEdge = new Edge { neighbour = newNode, isActive = true };
                edges.Add(newEdge);
            }
        }
    }

    // is a Node already in the Edges List?
    private bool HasNeighbour(Node node) {
        foreach (Edge e in edges) {
            if (e.neighbour != null && e.neighbour.Equals(node)) {
                return true;
            }
        }
        return false;
    }

    // given a specific neighbour, sets active state
    public void EnableEdge(Node neighbourNode, bool state) {
        foreach (Edge e in edges) {
            if (e.neighbour.Equals(neighbourNode)) {
                e.isActive = state;
            }
        }
    }

    public void InitGraph(Graph graphToInit) {
        this.graph = graphToInit;
    }
}
