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
    public float rotationSpeed = 1;
    private float progress = 0;
    private bool rotating = false;
    public NavController player;
    public PlayerAnimation playerAnimation;

    // Start is called before the first frame update
    void Start()
    {
        rotationLinker = GetComponent<RotationLinker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown("space")) {
            Rotate();
        }
        if (rotating) {
            doRotate();
            if (progress >= 1) {
                lockAngles();
                rotating = false;
                rotationLinker.UpdateRotationLinks();
                progress = 0;
            }
        }
    }

    void Rotate() {
        rotating = true;
    }

    void lockAngles() {
        foreach(RotationObj obj in rotateTogether) {
            obj.obj.transform.RotateAround(obj.rotatePoint.position, transform.up, -1 * obj.angle * (progress - 1));
        }
    }

    void doRotate() {
        player.SnapToCurrentNode();
        playerAnimation.ToggleAnimation(false);
        float inc = Time.deltaTime * rotationSpeed;
        foreach(RotationObj obj in rotateTogether) {
            obj.obj.transform.RotateAround(obj.rotatePoint.position, transform.up, obj.angle * inc);
        }
        progress += inc;
        
    }

    public override void Action(Vector3 position) {
        Rotate();
    }
}
