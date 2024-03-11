using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberProjection : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isUnlocked;
    public GameObject numberCard;
    public GameObject numberText;
    public Material textMaterialBefore;
    public Material textMaterialAfter;

    private Vector3[] initialVertices;
    private Vector3[] offsetVertices;
    public float offsetRange = 0.01f;

    void Start()
    {
        if (!isUnlocked) {
            /* Set material of text to textMaterialBefore */
            numberText.GetComponent<Renderer>().material = textMaterialBefore;
            /* Project numberCard */
            MeshFilter meshFilter = numberCard.GetComponentInChildren<MeshFilter>();

            Vector3[] vertices = meshFilter.mesh.vertices;
            int[] triangles = meshFilter.mesh.triangles;

            Vector3[] newVertices = new Vector3[triangles.Length * 3 * 2];
            int[] newTriangles = new int[triangles.Length * 3 * 2];

            // initialVertices = new Vector3[triangles.Length * 3];
            // offsetVertices = new Vector3[triangles.Length * 3];
            Vector3 splitAxis = new Vector3(1, 1, 1);
            splitAxis.Normalize();

            for(int i = 0; i < triangles.Length; i += 3) {
                float offset1 = Random.Range(0, offsetRange);
                float offset2 = Random.Range(0, offsetRange);

                /* Get vertices of Triangle */
                Vector3 v1a = vertices[triangles[i]];
                Vector3 v2a = vertices[triangles[i+1]];
                Vector3 v3a = vertices[triangles[i+2]];
                Vector3 v4a = (v1a + v3a)/2;

                /* Generate Random Direction */
                //splitAxis.Normalize();
                // initialVertices[i].x = v1a.x;
                // initialVertices[i].y = v1a.y;
                // initialVertices[i].z = v1a.z;

                // initialVertices[i+1].x = v2a.x;
                // initialVertices[i+1].y = v2a.y;
                // initialVertices[i+1].z = v2a.z;

                // initialVertices[i+2].x = v3a.x;
                // initialVertices[i+2].y = v3a.y;
                // initialVertices[i+2].z = v3a.z;

                Vector3 v1b = new Vector3(v1a.x, v1a.y, v1a.z);
                Vector3 v2ba = new Vector3(v2a.x, v2a.y, v2a.z);
                Vector3 v3b = new Vector3(v3a.x, v3a.y, v3a.z);
                Vector3 v4ba = new Vector3(v4a.x, v4a.y, v4a.z);
                Vector3 v4bb = new Vector3(v4a.x, v4a.y, v4a.z);
                Vector3 v2bb = new Vector3(v2a.x, v2a.y, v2a.z);
                v1b += offset1 * splitAxis;
                v2ba += offset1 * splitAxis;
                v4ba += offset1 * splitAxis;

                v2bb += offset1 * splitAxis;
                v4bb += offset1 * splitAxis;
                v3b += offset1 * splitAxis;

                /* Append vertices to newVertices */
                newVertices[i*2] = v1b;
                newVertices[i*2+1] = v2ba;
                newVertices[i*2+2] = v4ba;
                
                newVertices[i*2+3] = v2bb;
                newVertices[i*2+4] = v4bb;
                newVertices[i*2+5] = v3b;

                /* Append indexes to newTriangles */
                newTriangles[i*2] = i*2;
                newTriangles[i*2+1] = i*2+1;
                newTriangles[i*2+2] = i*2+2;

                newTriangles[i*2+3] = i*2+3;
                newTriangles[i*2+4] = i*2+4;
                newTriangles[i*2+5] = i*2+5;

                // offsetVertices[i] = new Vector3(v1b.x, v1b.y, v1b.z);
                // offsetVertices[i+1] = new Vector3(v2b.x, v2b.y, v2b.z);
                // offsetVertices[i+2] = new Vector3(v3b.x, v3b.y, v3b.z);
            }
            meshFilter.mesh.vertices = newVertices;
            meshFilter.mesh.triangles = newTriangles;
            meshFilter.mesh.RecalculateNormals();
            } else {
                /* Set material of text to textMaterialAfter */
                numberText.GetComponent<Renderer>().material = textMaterialAfter;
                /* Add idle spin */
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
