using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTranslate : BaseTouch
{
    protected Vector3 targetPos;

    // flag for whether this behaviour can be activated multiple times
    [SerializeField] private bool singleUse = false;

    // flag for whether this behaviour has been activated or not
    protected bool activated = false;

    // time that the movement should take
    [Range(0.25f, 2f)]
    [SerializeField] private float moveTime = 0.5f;

    private ShiftLinker shiftLinker;

    private void Awake()
    {
        shiftLinker = GetComponent<ShiftLinker>();
        targetPos = transform.position;
    }

    internal override void Action(Vector3 myInput)
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
        return;
    }

    //  lerp to destination from current position
    private IEnumerator MoveToPositionRoutine()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        // validate move time
        moveTime = Mathf.Clamp(moveTime, 0.1f, 5f);

        while (elapsedTime < moveTime && transform.position != targetPos)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = Mathf.Clamp(elapsedTime / moveTime, 0f, 1f);

            transform.position = Vector3.Lerp(startPosition, targetPos, lerpValue);

            // wait one frame
            yield return null;
        }

        if (shiftLinker != null)
        {
            shiftLinker.UpdateShiftLinks();
        }
    }
}
