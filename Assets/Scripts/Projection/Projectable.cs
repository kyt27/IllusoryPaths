using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectable : BaseEyeInteractable
{
    #region Fields
        public Transform m_obj;
        public GameObject split;
        public Transform m_split;
        public float eps1;
        public float eps2;
        public float offsetRange;
        public float mergeSpeed;
        public GameObject playButton;

        private Vector3 m_prevTouch = -Vector2.one;//screen coordinates
        private Camera camera;
        private Transform m_camera;
        private Vector3 splitAxis = new Vector3(0, 0, 1);
        private Vector3[] initialVertices;
        private Vector3[] offsetVertices;
        private bool merged = false;
        private bool merging = false;
    #endregion

    // IEnumerator switchPosition() {
    //     Mesh current = split.GetComponentInChildren<MeshFilter>().mesh;
    //     Vector3[] target = initialVertices;
    //     Vector3[] currentVertices = current.vertices;
    //     for (int i = 0; i < target.Length; i++) {
    //         // currentVertices[i].x = target[i].x;
    //         // currentVertices[i].y = target[i].y;
    //         // currentVertices[i].z = target[i].z;
    //         float move = mergeSpeed * Time.deltaTime;
    //         currentVertices[i] = Vector3.MoveTowards(currentVertices[i], target[i], move);
    //     }
    //     current.vertices = currentVertices;
    //     current.RecalculateNormals();
    //     current.RecalculateBounds();
    //     //merged = true;
    //     yield return null;
    // }

    void idleRotate() {
        float angle = 4 * Time.deltaTime;
        split.GetComponent<Transform>().Rotate(0, angle, 0);
    }

    public override void Action(Vector3 input) {

    }

    void displayPlayButton() {
        //GameObject.Find("PlayButton").GetComponent<Renderer>().enabled = true;
        // GameObject pButton = Instantiate(playButton, new Vector3(m_obj.position.x, m_obj.position.y - 1, m_obj.position.z), Quaternion.identity);
        // pButton.GetComponent
    }

    /* Bug in AR with Triangles */
    void mergeTrianglesPermanent() {
        Mesh current = split.GetComponentInChildren<MeshFilter>().mesh;
        Vector3[] target = initialVertices;
        Vector3[] currentVertices = current.vertices;
        for (int i = 0; i < target.Length; i++) {
            currentVertices[i].x = target[i].x;
            currentVertices[i].y = target[i].y;
            currentVertices[i].z = target[i].z;
        }
        current.vertices = currentVertices;
        current.RecalculateNormals();
    //     current.RecalculateBounds();
        merged = true;
        displayPlayButton();
    //     yield return null;
    }

    IEnumerator mergeTrianglesDynamic(float cosine_axis, float cosine_view) {
        Mesh current = split.GetComponentInChildren<MeshFilter>().mesh;
        Vector3[] target = initialVertices;
        Vector3[] currentVertices = current.vertices;
        for (int i = 0; i < target.Length; i++) {
            currentVertices[i] = Vector3.Lerp(offsetVertices[i], target[i], cosine_axis);
        }
        current.vertices = currentVertices;
        current.RecalculateNormals();
        current.RecalculateBounds();
        //merged = true;
        yield return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //camera = Camera.main;
        // GameObject.Find("PlayButton").GetComponent<Renderer>().enabled = false;
        m_camera = GameObject.Find("AR Camera").GetComponent<Transform>();

        MeshFilter meshFilter = split.GetComponentInChildren<MeshFilter>();

        Vector3[] vertices = meshFilter.mesh.vertices;
        int[] triangles = meshFilter.mesh.triangles;

        Vector3[] newVertices = new Vector3[triangles.Length * 3];
        int[] newTriangles = new int[triangles.Length * 3];

        initialVertices = new Vector3[triangles.Length * 3];
        offsetVertices = new Vector3[triangles.Length * 3];

        m_split.position = m_obj.TransformPoint(m_obj.position + splitAxis);

        for(int i = 0; i < triangles.Length; i += 3)
        {
            float offset = Random.Range(-1 * offsetRange, offsetRange);
            /* Get vertices of Triangle */
            Vector3 v1a = vertices[triangles[i]];
            Vector3 v2a = vertices[triangles[i+1]];
            Vector3 v3a = vertices[triangles[i+2]];

            initialVertices[i].x = v1a.x;
            initialVertices[i].y = v1a.y;
            initialVertices[i].z = v1a.z;

            initialVertices[i+1].x = v2a.x;
            initialVertices[i+1].y = v2a.y;
            initialVertices[i+1].z = v2a.z;

            initialVertices[i+2].x = v3a.x;
            initialVertices[i+2].y = v3a.y;
            initialVertices[i+2].z = v3a.z;

            Vector3 v1b = new Vector3(v1a.x, v1a.y, v1a.z);
            Vector3 v2b = new Vector3(v2a.x, v2a.y, v2a.z);
            Vector3 v3b = new Vector3(v3a.x, v3a.y, v3a.z);
            v1b += offset * splitAxis;
            v2b += offset * splitAxis;
            v3b += offset * splitAxis;
            
            /* Append vertices to newVertices */
            newVertices[i] = v1b;
            newVertices[i+1] = v2b;
            newVertices[i+2] = v3b;

            /* Append indexes to newTriangles */
            newTriangles[i] = i;
            newTriangles[i+1] = i+1;
            newTriangles[i+2] = i+2;

            offsetVertices[i] = new Vector3(v1b.x, v1b.y, v1b.z);
            offsetVertices[i+1] = new Vector3(v2b.x, v2b.y, v2b.z);
            offsetVertices[i+2] = new Vector3(v3b.x, v3b.y, v3b.z);
        }

        meshFilter.mesh.vertices = newVertices;
        meshFilter.mesh.triangles = newTriangles;
        meshFilter.mesh.RecalculateNormals();

        // GameObject pButton = Instantiate(playButton, new Vector3(m_obj.position.x, m_obj.position.y - 1, m_obj.position.z), Quaternion.identity);
        // pButton.GetComponent<Transform>().SetParent(m_obj);
        //meshFilter.mesh.RecalculateBounds();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMouseLeftDown = Input.GetMouseButton(0);

            if(isMouseLeftDown && m_prevTouch.x < 0)//click first time
            {
                m_prevTouch = Input.mousePosition;
            }
            else if(isMouseLeftDown && m_prevTouch.x >= 0)
            {
                Vector3 delta = Input.mousePosition - m_prevTouch;
                m_prevTouch = Input.mousePosition;
                m_obj.Rotate(delta.y, -delta.x, 0.0f, Space.World);
            }
            else
            {
                m_prevTouch = -Vector2.one;
            }

        Vector3 a = m_obj.TransformDirection(splitAxis);
        Vector3 b = m_camera.position - m_obj.position;
        float cosine_axis = Mathf.Abs(Vector3.Dot(a, b)/(a.magnitude * b.magnitude));
        float cosine_view = Mathf.Abs(Vector3.Dot(m_camera.forward, b)/(m_camera.forward.magnitude * b.magnitude));
        if (!merged) {
            StartCoroutine(mergeTrianglesDynamic(cosine_axis, cosine_view));
        } else {
            idleRotate();
        }
        if (!merged && 1 - cosine_axis < eps1 /* Fix this && 1 - cosine_view < eps2*/) {
            mergeTrianglesPermanent();
        }
        // if (merging || (1 - cosine_axis < eps1 && 1 - cosine_view < eps2)) {
        //     merging = true;
        //     //switchPosition(initialVertices, split.GetComponentInChildren<MeshFilter>().mesh);
        //     StartCoroutine(switchPosition());
        //     // split.GetComponent<Renderer>().enabled = false;
        //     // after.GetComponent<Renderer>().enabled = true;
        // }
    }
}
