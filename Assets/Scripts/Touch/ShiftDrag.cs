using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class ShiftDrag : BaseTouch {

    public bool xDirection;
    public bool yDirection;
    public bool zDirection;

    public GameObject[] shiftTogether;
    
    private bool isDragging = false;

    public bool autoDetectEnds = true;
    public float maxDist = 10f;

    [SerializeField] private Vector3 posEnd;
    [SerializeField] private Vector3 negEnd;

    
    private ShiftLinker shiftLinker;

    private Vector3 initialPos;

    internal override void Action(Vector3 myInput) {
        actionPersist = true;
        isDragging = true;
    }

    internal override void Initialise() {
        shiftLinker = GetComponent<ShiftLinker>();

        if(autoDetectEnds) {
            FindEnd();
        }

        if(!xDirection && !yDirection && !zDirection) {
            Debug.Log("NO DIRECTION SET ON SHIFTING OBJECT: " + transform.name);
        }

        initialPos = transform.position;
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
            transform.position = posEnd;
        } else if (k > 1) {
            transform.position = negEnd;
        } else {
            transform.position = posEnd - k * (posEnd - negEnd);
        }
    }

    void StopDrag() {
        isDragging = false;
        actionPersist = false;

        float x = initialPos.x - Mathf.Floor(initialPos.x);
        float y = initialPos.y - Mathf.Floor(initialPos.y);
        float z = initialPos.z - Mathf.Floor(initialPos.z);

        if(x < 0.1f) {
            x = Mathf.Round(transform.position.x);
        } else {
            x = Mathf.Floor(transform.position.x) + 0.5f;
        }

        if(y < 0.1f) {
            y = Mathf.Round(transform.position.y);
        } else {
            y = Mathf.Floor(transform.position.y) + 0.5f;
        }

        if(z < 0.1f) {
            z = Mathf.Round(transform.position.z);
        } else {
            z = Mathf.Floor(transform.position.z) + 0.5f;
        }

        transform.position = new Vector3(x, y, z);

        if(shiftLinker == null) return;

        shiftLinker.UpdateShiftLinks();
    }

    void FindEnd() {
        float ctr = 0f;

        Vector3 maxCoords = GetComponent<Collider>().bounds.max;
        Vector3 minCoords = GetComponent<Collider>().bounds.min;

        if (xDirection) {
            posEnd = transform.position + new Vector3(maxDist, 0, 0);
            negEnd = transform.position + new Vector3(-maxDist, 0, 0);

            ctr = Mathf.Ceil(maxCoords.x);
            for(float x=0; x<maxDist; x++) {
                Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x + ctr, transform.position.y, transform.position.z), 0.49f);
                if(hitColliders.Length > 0) {
                    posEnd = transform.position + new Vector3(x, 0, 0);
                    break;
                }
            }
            ctr = Mathf.Floor(minCoords.x);
            for(float x=0; x>-maxDist; x--) {
                Collider[] hitColliders = Physics.OverlapSphere(new Vector3(x + ctr, transform.position.y, transform.position.z), 0.49f);
                if(hitColliders.Length > 0) {
                    negEnd = transform.position + new Vector3(x, 0, 0);
                    break;
                }
            }
            
        } else if (yDirection) {
            posEnd = transform.position + new Vector3(0, maxDist, 0);
            negEnd = transform.position + new Vector3(0, -maxDist, 0);
            
            ctr = Mathf.Ceil(maxCoords.y);
            for(float y=0; y<maxDist; y++) {
                Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, y + ctr, transform.position.z), 0.49f);
                if(hitColliders.Length > 0) {
                    posEnd = transform.position + new Vector3(0, y, 0);
                    return;
                }
            }
            ctr = Mathf.Floor(minCoords.y);
            for(float y=0; y>-maxDist; y--) {
                Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, y + ctr, transform.position.z), 0.49f);
                if(hitColliders.Length > 0) {
                    negEnd = transform.position + new Vector3(0, y, 0);
                    return;
                }
            }
        } else if (zDirection) {
            posEnd = transform.position + new Vector3(0, 0, maxDist);
            negEnd = transform.position + new Vector3(0, 0, -maxDist);

            ctr = Mathf.Ceil(maxCoords.z);
            for(float z=0; z<maxDist; z++) {
                Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, z + ctr), 0.49f);
                if(hitColliders.Length > 0) {
                    posEnd = transform.position + new Vector3(0, 0, z);
                    return;
                }
            }
            ctr = Mathf.Floor(minCoords.z);
            for(float z=0; z>-maxDist; z--) {
                Collider[] hitColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, z + ctr), 0.49f);
                if(hitColliders.Length > 0) {
                    negEnd = transform.position + new Vector3(0, 0, z);
                    return;
                }
            }
        }
    }
}
