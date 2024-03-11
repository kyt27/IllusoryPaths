using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GlobalAlign : MonoBehaviour {
    /*
    Quaternion rotation;

    void Awake() {
        AddNodes[] addNodeScripts = GetComponentsInChildren<AddNodes>();

        GlobalAlign globalAlign = GetComponent<GlobalAlign>();

        Debug.Log("start: " + addNodeScripts[0].transform.rotation);

        foreach(AddNodes addNodes in addNodeScripts) {
            addNodes.Align();
        }

        globalAlign.Unalign();

        foreach(AddNodes addNodes in addNodeScripts) {
            addNodes.RunAddNodesAwake();
        }

        Debug.Log("middle: " + addNodeScripts[0].transform.rotation);
        
        Debug.Log(addNodeScripts[0].transform.rotation);

        globalAlign.Realign();
    }

    public void Unalign() {
        rotation = this.transform.localRotation;
        this.transform.localRotation = Quaternion.identity;
    }

    public void Realign() {
        // this.transform.rotation = rotation;
    }
    */
}
