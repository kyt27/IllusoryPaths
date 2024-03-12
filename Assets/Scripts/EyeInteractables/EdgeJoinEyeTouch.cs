using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class of Node pairs to line up
[System.Serializable]
public class NodePair
{
    public Node nodeA;
    public Node nodeB;
}

public class EdgeJoinEyeTouch : MonoBehaviour
{
    [SerializeField] private List<Material> eyeMaterials;

    [SerializeField] private float hintError = 5.0f;
    [SerializeField] private float acceptError = 1.0f;

    private bool interactable = false;

    [SerializeField] private List<NodePair> nodePairs;

    [SerializeField] private GameObject controlledObjects;

    private bool active = true;

    private Camera myCamera;
    private Vector3 normal;
    private Ray ray;
    private RaycastHit hit;

    private bool debugPrinted = false;

    [SerializeField] private bool testInteractable = false;
    public BaseEyeInteractable eyeGlow;

    void Start()
    {
        myCamera = Camera.main;

        if (controlledObjects == null) throw new System.Exception("no object assigned to eye");
        if (controlledObjects.GetComponent<BaseEyeInteractable>() == null) throw new System.Exception("assigned object needs to contain script extending BaseEyeInteractable");
    }

    void Update()
    {
        float distance = 0;

        foreach (NodePair n in nodePairs)
        {
            if (n.nodeA == null || n.nodeB == null) continue;
            distance += (myCamera.WorldToScreenPoint(n.nodeA.transform.position)
                - myCamera.WorldToScreenPoint(n.nodeB.transform.position)).magnitude;
        }

        if (!debugPrinted)
        {
            Debug.Log("distance between nodes: " + distance);
            debugPrinted = true;
        }


        if (testInteractable)
        {
            GetComponent<MeshRenderer>().material = eyeMaterials[2];
            interactable = true;
        }
        else if (distance > hintError)
        {
            GetComponent<MeshRenderer>().material = eyeMaterials[0];
            interactable = false;
        }
        else if (distance > acceptError)
        {
            GetComponent<MeshRenderer>().material = eyeMaterials[1];
            interactable = false;
        }
        else
        {
            GetComponent<MeshRenderer>().material = eyeMaterials[2];
            interactable = true;
        }

        if (interactable)
        {
            IsClickObject();
        }
    }

    void HandleRayCast(Vector3 myInput)
    {
        ray = myCamera.ScreenPointToRay(myInput);
        if (Physics.Raycast(ray, out hit))
        {
            if (GameObject.ReferenceEquals(hit.transform.gameObject, this.gameObject))
            {
                transform.parent.gameObject.transform.parent.gameObject.GetComponentsInChildren<NavController>()[0].GetComponent<NavController>().SnapToCurrentNode();
                controlledObjects.GetComponent<BaseEyeInteractable>().Action(myInput);
                eyeGlow.Action(myInput);
            }
        }
    }


    void HandleClickMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleRayCast(Input.mousePosition);
        }
    }

    void HandleTouchMobile()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            HandleRayCast(Input.touches[0].position);
        }
    }

    void IsClickObject()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleClickMouse();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchMobile();
#endif
    }
}
