using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavmeshRefresh : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface navMeshSurface;

    // Update is called once per frame
    void Update()
    {
        navMeshSurface.BuildNavMesh();
    }
}
