﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTransformObject : BaseEyeInteractable
{
    // flag for whether this behaviour can be activated multiple times
    [SerializeField] private bool singleUse = false;

    // flag for whether this behaviour has been activated or not
    protected bool activated = false;

    // time that the movement should take
    [Range(0.25f, 2f)]
    [SerializeField] private float moveTime = 0.5f;

    // GameObject that this object should transform into
    [SerializeField] private GameObject target;

    private ShiftLinker shiftLinker;

    // target transform - NO rotation supported for now
    private Vector3 targetPos;
    private Vector3 targetScale;

    //private void Start()
    //{
    //    UpdateTarget();
    //}

    private void Awake()
    {
        shiftLinker = GetComponent<ShiftLinker>();
        UpdateTarget();
        target.SetActive(false);
        //target.GetComponent<Renderer>().enabled = false;
    }

    public override void Action(Vector3 myInput)
    {
        if (!singleUse || !activated)
        {
            UpdateTarget();
            StartCoroutine(MoveToPositionRoutine());
            activated = true;
        }
    }

    internal virtual void UpdateTarget()
    {
        targetPos = target.transform.position;

        Vector3 targetDims = target.GetComponent<Renderer>().bounds.size;
        Vector3 startDims = transform.GetComponent<Renderer>().bounds.size;
        targetScale.x = targetDims.x / startDims.x;
        targetScale.y = targetDims.y / startDims.y;
        targetScale.z = targetDims.z / startDims.z;

        // scale back to original size
        targetScale.Scale(transform.localScale);

        //Debug.Log("target dims: " + targetDims + "current dims: " + transform.GetComponent<Renderer>().bounds.size);
    }


    //  lerp to destination from current position
    private IEnumerator MoveToPositionRoutine()
    {
        Debug.Log("currently at " + transform.position + ", moving to " + targetPos);
        Debug.Log("current size " + transform.GetComponent<Renderer>().bounds.size + ", scaling to " + targetScale);

        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;

        float elapsedTime = 0;

        // validate move time
        moveTime = Mathf.Clamp(moveTime, 0.1f, 5f);

        while (elapsedTime < moveTime && transform.position != targetPos)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = Mathf.Clamp(elapsedTime / moveTime, 0f, 1f);

            // update position
            transform.position = Vector3.Lerp(startPosition, targetPos, lerpValue);

            // update scale
            transform.localScale = Vector3.Lerp(startScale, targetScale, lerpValue);

            // wait one frame
            yield return null;
        }

        if (shiftLinker != null)
        {
            shiftLinker.ActivateAllShiftLinks();
        }
    }
}
