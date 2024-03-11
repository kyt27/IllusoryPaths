/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using TMPro.Examples;

public class BlinkScript : MonoBehaviour {
    private PostProcessVolume ppVolume;

    public GameObject topLid;
    public GameObject bottomLid;

    private bool blink = false;

    void Start () {
        ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
    }

    public void Blink() {
        ppVolume.enabled = !ppVolume.enabled;
        blink = !blink;
        if(blink) {
            topLid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);
            bottomLid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -300);
        } else {
            topLid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 3000);
            bottomLid.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -3000);
        }
    }
}
*/