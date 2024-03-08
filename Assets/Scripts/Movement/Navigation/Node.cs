using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    // gizmo colors
    [SerializeField] private float gizmoRadius = 0.1f;
    [SerializeField] private Color defaultGizmoColor = Color.black;
    [SerializeField] private Color selectedGizmoColor = Color.blue;
    [SerializeField] private Color inactiveGizmoColor = Color.gray;

    [SerializeField] private bool enabled = true;

    // neighboring nodes + active state
    [SerializeField] private List<Edge> edges = new List<Edge>();

    // Nodes specifically excluded from Edges
    [SerializeField] private List<Node> excludedNodes;

    // reference to the graph
    private Graph graph;

    // previous Node that forms a "breadcrumb" trail back to the start
    private Node previousNode;

    public List<GameObject> interactables;
    
    public Node PreviousNode { get { return previousNode; } set { previousNode = value; } }
    public List<Edge> Edges => edges;

    [SerializeField] private bool posX;
    [SerializeField] private bool negX;
    [SerializeField] private bool posY;
    [SerializeField] private bool negY;
    [SerializeField] private bool posZ;
    [SerializeField] private bool negZ;

    // 3d compass directions to check for all neighbors automatically
    public static Vector3[] xNeighbourDirections = {
        new Vector3(0f, 1f, 0f),
        new Vector3(0f, -1f, 0f),
        new Vector3(0f, 0f, 1f),
        new Vector3(0f, 0f, -1f),
    };

    public static Vector3[] yNeighbourDirections = {
        new Vector3(1f, 0f, 0f),
        new Vector3(-1f, 0f, 0f),
        new Vector3(0f, 0f, 1f),
        new Vector3(0f, 0f, -1f),
    };

    public static Vector3[] zNeighbourDirections = {
        new Vector3(1f, 0f, 0f),
        new Vector3(-1f, 0f, 0f),
        new Vector3(0f, 1f, 0f),
        new Vector3(0f, -1f, 0f),
    };
        
    private void Start() {
        if (!posX && !negX && !posY && !negY && !posZ && !negZ) {
            posY = true;
        }

        // automatic connect Edges with horizontal Nodes
        if (graph != null) {
            FindNeighbours();
        }

        if(!enabled) DisableNode();
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
        Vector3[] neighbourDirections = new Vector3[4];
        if (posX || negX) {
            neighbourDirections = xNeighbourDirections;
        } else if(posY || negY) {
            neighbourDirections = yNeighbourDirections;
        } else if(posZ || negZ) {
            neighbourDirections = zNeighbourDirections;
        }
        
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
        bool exists = false;

        foreach (Edge e in edges) {
            if (e.neighbour.Equals(neighbourNode)) {
                exists = true;
                e.isActive = state;
            }
        }

        if(!exists) {
            Edge newEdge = new Edge { neighbour = neighbourNode, isActive = state };
            edges.Add(newEdge);
        }
    }

    public void InitGraph(Graph graphToInit) {
        this.graph = graphToInit;
    }

    public void Interact() {
        foreach(GameObject obj in interactables) {
            obj.GetComponent<BaseNodeInteractable>().Action(transform.position);
        }
    }

    public void DisableNode() {
        foreach (Edge e in edges) {
            e.isActive = false;
            e.neighbour.EnableEdge(this, false);
        }
    }

    public void EnableNode() {
        foreach (Edge e in edges) {
            e.isActive = true;
            e.neighbour.EnableEdge(this, true);
        }
    }

    public void SetDirection(string direction) {
        posX = false;
        negX = false;
        posY = false;
        negY = false;
        posZ = false;
        negZ = false;
        switch(direction) {
            case "posX":
                posX = true;
                break;
            case "negX":
                negX = true;
                break;
            case "posY":
                posY = true;
                break;
            case "negY":
                negY = true;
                break;
            case "posZ":
                posZ = true;
                break;
            case "negZ":
                negZ = true;
                break;
            default:
                Debug.Log("NO VALID DIRECTION");
                break;
        }
    }
}
