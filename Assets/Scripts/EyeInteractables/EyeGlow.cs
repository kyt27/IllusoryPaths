using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGlow : BaseEyeInteractable
{

    public Material glowMaterial;
    private bool isGlowing;
    public Color glowColor;
    public float glowIntensity;
    public float fadeSpeed;
    float eps = 0.0001f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = glowMaterial;
        isGlowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGlowing)
            diminishGlow();
    }

    void diminishGlow() {
        glowIntensity -= fadeSpeed * Time.deltaTime;
        glowMaterial.SetColor("_EmissionColor", glowColor * glowIntensity);
        if (glowIntensity < eps)
            isGlowing = false;
    }
    
    void activateGlow() {
        glowMaterial.SetColor("_EmissionColor", glowColor * glowIntensity);
        isGlowing = true;
    }

    public override void Action(Vector3 input) {
        activateGlow();
    }
}
