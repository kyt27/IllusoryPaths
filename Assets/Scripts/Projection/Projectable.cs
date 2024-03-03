using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectable : MonoBehaviour
{
    #region Fields
        public Transform m_obj;
        public GameObject split;
        public GameObject after;
        public Transform m_camera;
        public Transform m_split;

        private Vector3 m_prevTouch = -Vector2.one;//screen coordinates
        private Vector3 splitAxis = new Vector3(0, 1, 0);
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        after.GetComponent<Renderer>().enabled = false;

        MeshFilter meshFilter = split.GetComponentInChildren<MeshFilter>();

        Vector3[] vertices = meshFilter.mesh.vertices;
        int[] triangles = meshFilter.mesh.triangles;

        Vector3[] newVertices = new Vector3[triangles.Length * 3];
        int[] newTriangles = new int[triangles.Length * 3];

        m_split.position = m_obj.position + splitAxis;

        for(int i = 0; i < triangles.Length; i += 3)
        {
            float yOffset = Random.Range(-2f, 2f);
            /* Get vertices of Triangle */
            Vector3 v1a = vertices[triangles[i]];
            Vector3 v2a = vertices[triangles[i+1]];
            Vector3 v3a = vertices[triangles[i+2]];

            Vector3 v1b = new Vector3(v1a.x, v1a.y + yOffset, v1a.z);
            Vector3 v2b = new Vector3(v2a.x, v2a.y + yOffset, v2a.z);;
            Vector3 v3b = new Vector3(v3a.x, v3a.y + yOffset, v3a.z);;
            
            /* Append vertices to newVertices */
            newVertices[i] = v1b;
            newVertices[i+1] = v2b;
            newVertices[i+2] = v3b;

            /* Append indexes to newTriangles */
            newTriangles[i] = i;
            newTriangles[i+1] = i+1;
            newTriangles[i+2] = i+2;
        }

        meshFilter.mesh.vertices = newVertices;
        meshFilter.mesh.triangles = newTriangles;
        meshFilter.mesh.RecalculateNormals();
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

        Vector3 a = m_split.position - m_obj.position;
        Vector3 b = m_camera.position - m_obj.position;
        float cosine_axis = Mathf.Abs(Vector3.Dot(a, b)/(a.magnitude * b.magnitude));
        float cosine_view = Mathf.Abs(Vector3.Dot(m_camera.forward, b)/(m_camera.forward.magnitude * b.magnitude));

        if (1 - cosine_axis < 0.004 && 1 - cosine_view < 0.004) {
            split.GetComponent<Renderer>().enabled = false;
            after.GetComponent<Renderer>().enabled = true;
        }
    }
}
