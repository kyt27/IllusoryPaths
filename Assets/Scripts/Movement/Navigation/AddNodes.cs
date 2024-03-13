using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddNodes : MonoBehaviour {
    [SerializeField] private bool posX;
    [SerializeField] private bool negX;
    [SerializeField] private bool posY;
    [SerializeField] private bool negY;
    [SerializeField] private bool posZ;
    [SerializeField] private bool negZ;

    public List<Vector3> excluded;

    private GameObject node;

    /*
    private Vector3 curAngle;
    private Vector3 parentAngle;

    public static float GetXDegrees(Transform t) {
        float radians = Mathf.Atan2(t.forward.y, -t.forward.z);
        return 180f + radians * Mathf.Rad2Deg;
    }

    public void Align() {
        curAngle = new Vector3(GetXDegrees(this.transform), this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z);
        parentAngle = new Vector3(GetXDegrees(transform.parent.transform), transform.parent.transform.rotation.eulerAngles.y, transform.parent.transform.rotation.eulerAngles.z);

        transform.Rotate(parentAngle - curAngle);
    }
    */

    // public void RunAddNodesAwake() {
    public void Awake() {
        if (!posX && !negX && !posY && !negY && !posZ && !negZ) {
            posY = true;
        }

        node = new GameObject("Node");
        node.AddComponent<Node>();

        AddExcludedNodes();

        Vector3 minCoords = GetComponent<Collider>().bounds.min;
        Vector3 maxCoords = GetComponent<Collider>().bounds.max;

        // Debug.Log(transform.name + " " + minCoords);
        // Debug.Log(transform.name + " " + maxCoords);

        SpawnNodes(minCoords, maxCoords);

        // transform.Rotate(curAngle - parentAngle);

        Destroy(node);
    }

    void AddExcludedNodes() {
        List<Node> allNodes = GetComponentsInChildren<Node>().ToList();
        foreach(Node n in allNodes) {
            excluded.Add(n.transform.position);
            // Debug.Log("excluded: " + n.transform.position);
        }
    }

    void SpawnNodes(Vector3 minCoords, Vector3 maxCoords) {
        if(posX) {
            for(float y=Mathf.Ceil(minCoords.y); y<=Mathf.Floor(maxCoords.y); y++) {
                for(float z=Mathf.Ceil(minCoords.z); z<=Mathf.Floor(maxCoords.z); z++) {
                    bool empty = false;
                    for(float x=Mathf.Ceil(maxCoords.x); x>=Mathf.Ceil(minCoords.x); x--) {
                        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, y, z), 0.49f);
                        if(hitColliders.Length == 0) {
                            empty = true;
                        } else {
                            if(empty) AttachNode(new Vector3(x + 0.5f, y, z), Quaternion.LookRotation(new Vector3(1, 0, 0)), "posX");
                            break;
                        }
                    }
                } 
            }
        }
        if(negX) {
            for(float y=Mathf.Ceil(minCoords.y); y<=Mathf.Floor(maxCoords.y); y++) {
                for(float z=Mathf.Ceil(minCoords.z); z<=Mathf.Floor(maxCoords.z); z++) {
                    bool empty = false;
                    for(float x=Mathf.Floor(minCoords.x); x<=Mathf.Floor(maxCoords.x); x++) {
                        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, y, z), 0.49f);
                        if(hitColliders.Length == 0) {
                            empty = true;
                        } else {
                            if(empty) AttachNode(new Vector3(x - 0.5f, y, z), Quaternion.LookRotation(new Vector3(-1, 0, 0)), "negX");
                            break;
                        }
                    }
                } 
            }
        }
        if(posY) {
            for(float x=Mathf.Ceil(minCoords.x); x<=Mathf.Floor(maxCoords.x); x++) {
                for(float z=Mathf.Ceil(minCoords.z); z<=Mathf.Floor(maxCoords.z); z++) {
                    bool empty = false;
                    for(float y=Mathf.Ceil(maxCoords.y); y>=Mathf.Ceil(minCoords.y); y--) {
                        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, y, z), 0.49f);
                        if(hitColliders.Length == 0) {
                            empty = true;
                        } else {
                            if(empty) AttachNode(new Vector3(x, y + 0.5f, z), Quaternion.LookRotation(new Vector3(0, 1, 0)), "posY");
                            break;
                        }
                    }
                } 
            }
        }
        if(negY) {
            for(float x=Mathf.Ceil(minCoords.x); x<=Mathf.Floor(maxCoords.x); x++) {
                for(float z=Mathf.Ceil(minCoords.z); z<=Mathf.Floor(maxCoords.z); z++) {
                    bool empty = false;
                    for(float y=Mathf.Floor(minCoords.y); y<=Mathf.Ceil(maxCoords.y); y++) {
                        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, y, z), 0.49f);
                        if(hitColliders.Length == 0) {
                            empty = true;
                        } else {
                            if(empty) AttachNode(new Vector3(x, y - 0.5f, z), Quaternion.LookRotation(new Vector3(0, -1, 0)), "negY");
                            break;
                        }
                    }
                } 
            }
        }
        if(posZ) {
            for(float x=Mathf.Ceil(minCoords.x); x<=Mathf.Floor(maxCoords.x); x++) {
                for(float y=Mathf.Ceil(minCoords.y); y<=Mathf.Floor(maxCoords.y); y++) {
                    bool empty = false;
                    for(float z=Mathf.Ceil(maxCoords.z); z>=Mathf.Ceil(minCoords.z); z--) {
                        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, y, z), 0.49f);
                        if(hitColliders.Length == 0) {
                            empty = true;
                        } else {
                            if(empty) AttachNode(new Vector3(x, y, z + 0.5f), Quaternion.LookRotation(new Vector3(0, 0, 1)), "posZ");
                            break;
                        }
                    }
                } 
            }
        }
        if(negZ) {
            for(float x=Mathf.Ceil(minCoords.x); x<=Mathf.Floor(maxCoords.x); x++) {
                for(float y=Mathf.Ceil(minCoords.y); y<=Mathf.Floor(maxCoords.y); y++) {
                    bool empty = false;
                    for(float z=Mathf.Floor(minCoords.z); z<=Mathf.Floor(maxCoords.z); z++) {
                        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x, y, z), 0.49f);
                        if(hitColliders.Length == 0) {
                            empty = true;
                        } else {
                            if(empty) AttachNode(new Vector3(x, y, z - 0.5f), Quaternion.LookRotation(new Vector3(0, 0, -1)), "negZ");
                            break;
                        }
                    }
                } 
            }
        }
    }

    void AttachNode(Vector3 pos, Quaternion rotation, string direction) {   
        if(excluded.Any(item => item == pos)) return;

        node.GetComponent<Node>().SetDirection(direction);

        Instantiate(node, pos, rotation, this.transform);
        //Node temp = Instantiate(node, this.transform, false);
        //temp.localPosition = pos;
        //temp.localRotation = rotation;
    }
}
