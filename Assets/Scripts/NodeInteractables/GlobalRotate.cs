using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotationObj {
    public GameObject obj;
    public Vector3 axis;
    public float angle;
    public Transform rotatePoint;
}

public class GlobalRotate : BaseNodeInteractable
{

    public List<RotationObj> rotateTogether = new List<RotationObj>();
    private RotationLinker rotationLinker;
    // Start is called before the first frame update
    void Start()
    {
        rotationLinker = GetComponent<RotationLinker>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            Rotate();
        }
    }

    void Rotate() {
        foreach(RotationObj obj in rotateTogether) {
            obj.obj.transform.RotateAround(obj.rotatePoint.position, obj.axis, obj.angle);
        }
        rotationLinker.UpdateRotationLinks();
    }

    public override void Action(Vector3 position) {
        Rotate();
    }
}
