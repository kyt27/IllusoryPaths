using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftDrag : BaseTouch {

    public bool xDirection;
    public bool yDirection;
    public bool zDirection;

    public GameObject[] shiftTogether;
    
    private bool isDragging = false;

    public float posDist = 4f;

    public float negDist = 4f;

    private Vector3 posEnd;
    private Vector3 negEnd;
    
    private ShiftLinker shiftLinker;

    private Vector3 initialPos;

    internal override void Action(Vector3 myInput) {
        actionPersist = true;
        isDragging = true;
    }

    internal override void Initialise() {
        shiftLinker = GetComponent<ShiftLinker>();

        if(!xDirection && !yDirection && !zDirection) {
            Debug.Log("NO DIRECTION SET ON SHIFTING OBJECT: " + transform.name);
            xDirection = true;
        }

        if(negDist < 0) {
            negDist = -negDist;
        }

        SetEnds();

        initialPos = transform.localPosition;
    }

    internal override void TogglePersist() {
        # if UNITY_EDITOR || UNITY_STANDALONE
        HandleDragMouse();
        # elif UNITY_ANDROID || UNITY_IOS
        HandleDragMobile();
        # endif
    }

    void HandleDragMouse() {
        if(Input.GetMouseButtonUp(0)) {
            StopDrag();
        }
        if(isDragging) {
            DragTo(Input.mousePosition);
        }
    }

    void HandleDragMobile() {
        if (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Ended) {
            StopDrag();
        }
        if(isDragging) {
            DragTo(Input.touches[0].position);
        }
    }
    
    void DragTo(Vector3 currentPosition) {
        Vector2 a = Camera.main.WorldToScreenPoint(posEnd);
        Vector2 b = Camera.main.WorldToScreenPoint(negEnd);

        Vector2 p = currentPosition;

        Vector2 r = a - b;

        float k = Vector2.Dot(a - p, r) / Vector2.Dot(r, r);

        if (k < 0) {
            transform.localPosition = posEnd;
        } else if (k > 1) {
            transform.localPosition = negEnd;
        } else {
            transform.localPosition = posEnd - k * (posEnd - negEnd);
        }
    }

    void StopDrag() {
        isDragging = false;
        actionPersist = false;

        float x = initialPos.x - Mathf.Floor(initialPos.x);
        float y = initialPos.y - Mathf.Floor(initialPos.y);
        float z = initialPos.z - Mathf.Floor(initialPos.z);

        if(x < 0.1f) {
            x = Mathf.Round(transform.localPosition.x);
        } else {
            x = Mathf.Floor(transform.localPosition.x) + 0.5f;
        }

        if(y < 0.1f) {
            y = Mathf.Round(transform.localPosition.y);
        } else {
            y = Mathf.Floor(transform.localPosition.y) + 0.5f;
        }

        if(z < 0.1f) {
            z = Mathf.Round(transform.localPosition.z);
        } else {
            z = Mathf.Floor(transform.localPosition.z) + 0.5f;
        }

        transform.localPosition = new Vector3(x, y, z);

        if(shiftLinker == null) return;

        shiftLinker.UpdateShiftLinks();
    }

    void SetEnds() {
        if (xDirection) {
            posEnd = transform.localPosition + new Vector3(posDist, 0, 0);
            negEnd = transform.localPosition - new Vector3(negDist, 0, 0);
        } else if (yDirection) {
            posEnd = transform.localPosition + new Vector3(0, posDist, 0);
            negEnd = transform.localPosition - new Vector3(0, negDist, 0);
        } else if (zDirection) {
            posEnd = transform.localPosition + new Vector3(0, 0, posDist);
            negEnd = transform.localPosition - new Vector3(0, 0, negDist);
        }
    }
}
