/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    private Camera myCamera;

    [SerializeField]
    private NavMeshAgent agent;

    void Start()
    {
        myCamera = Camera.main;
    }

    void Update()
    {
        IsClickObject();
    }


    void HandleRayCast(Vector3 myInput)
    {
        Ray movePosition = myCamera.ScreenPointToRay(myInput);
        if (Physics.Raycast(movePosition, out var hitInfo))
        {
            agent.SetDestination(hitInfo.point);
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
    # elif UNITY_ANDROID || UNITY_IOS
        HandleTouchMobile();
    #endif
    }
}
*/
