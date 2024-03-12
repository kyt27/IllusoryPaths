using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCountEnable : CountEnable {
    internal override void Action() {
        GetComponent<EyeTouch>().enabled = true;
    }
}
