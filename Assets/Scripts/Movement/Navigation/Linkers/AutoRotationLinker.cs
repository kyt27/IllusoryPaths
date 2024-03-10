using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;

[RequireComponent(typeof(RotateDrag))]
public class AutoRotationLinker : MonoBehaviour{
    private List<Node> allNodes = new List<Node>();

    private RotateDrag rotateDrag;

    private List<RotationLink> rotationLinks;

    private GraphHelper graphHelper;

    private void Start() {
        rotateDrag = GetComponent<RotateDrag>();
        rotationLinks = new List<RotationLink>();

        allNodes = GetComponentsInChildren<Node>().ToList();
        foreach (GameObject obj in rotateDrag.rotateTogether) {
            allNodes.AddRange(obj.GetComponentsInChildren<Node>().ToList());
        }

        graphHelper = transform.parent.gameObject.GetComponent<GraphHelper>();
        
        AddLinkers();
    }

    private void AddLinkers() {
        float curRotation = 0f;
        Vector3 direction = rotateDrag.GetDirection();

        foreach (float rotation in rotateDrag.snapAngles) {
            foreach(GameObject obj in rotateDrag.rotateTogether) {
                obj.transform.Rotate(direction, rotation - curRotation);
            }
            transform.Rotate(direction, rotation - curRotation);

            // Debug.Log(rotation * direction);
            foreach(Node node in allNodes) {
                // FindNeighboursToLink(node, new Vector3(RotateDrag.GetXDegrees(this.transform), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
                FindNeighboursToLink(node, rotation * direction);
            }

            curRotation = rotation;
        }

        foreach(GameObject obj in rotateDrag.rotateTogether) {
            obj.transform.Rotate(direction, 360f - curRotation);
        }
        transform.Rotate(direction, 360f - curRotation);

        RotationLinker rotationLinker = GetComponent<RotationLinker>();
        if(rotationLinker == null) {
            rotationLinker = gameObject.AddComponent<RotationLinker>();
            rotationLinker.rotationLinks = rotationLinks.ToArray();
        } else {
            rotationLinker.rotationLinks = rotationLinker.rotationLinks.ToList().Concat(rotationLinks).ToArray();
        }
    }

    private Node FindNodeAt(Vector3 pos) {
        foreach (Node n in graphHelper.allNodes) {
            Vector3 diff = n.transform.position - pos;

            if (diff.sqrMagnitude < 0.01f) return n;
        }
        return null;
    }

    private void FindNeighboursToLink(Node curNode, Vector3 activeEulerAngle) {
        // search through possible neighbour offsets
        Vector3[] neighbourDirections = {
            new Vector3(1f, 0f, 0f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, -1f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, -1f),
        };

        foreach (Vector3 direction in neighbourDirections) {
            Node newNode = FindNodeAt(curNode.transform.position + direction);
            if(newNode != null) {
                bool searchOnSelf = false;
                foreach (Node n in allNodes) {
                    if(n.transform.position == newNode.transform.position) {
                        searchOnSelf = true;
                        break;
                    }          
                }

                // add to edges list if not already included and not excluded specifically
                if(!searchOnSelf) rotationLinks.Add(new RotationLink {activeEulerAngle = activeEulerAngle, nodeA = curNode, nodeB = newNode});
            }
        }
    }
}

