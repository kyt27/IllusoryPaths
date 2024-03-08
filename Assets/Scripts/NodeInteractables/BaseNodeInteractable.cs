using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNodeInteractable : MonoBehaviour {
    public abstract void Action(Vector3 myInput);
}