using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountEnable : MonoBehaviour {
    public int numKeys;
    public int curCount = 0;
    
    void Awake() {
        if(numKeys == 0) Debug.Log("NUMKEYS NOT SET IN COUNTENABLE");
    }

    internal abstract void Action();

    public void increaseCount() {
        curCount++;
        if(curCount == numKeys) {
            Action();
        }
    }
}
