using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LightEstimation : MonoBehaviour
{

    public ARCameraManager arcm;
    Light light;

    void OnEnable() {
        arcm.frameReceived += getLight;
    }

    void OnDisable() {
        arcm.frameReceived -= getLight;
    }

    /// <summary>
    /// The estimated brightness of the physical environment, if available.
    /// </summary>
    public float? brightness { get; private set; }

    /// <summary>
    /// The estimated color temperature of the physical environment, if available.
    /// </summary>
    public float? colorTemperature { get; private set; }

    /// <summary>
    /// The estimated color correction value of the physical environment, if available.
    /// </summary>
    public Color? colorCorrection { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    void getLight(ARCameraFrameEventArgs args) {
        if (args.lightEstimation.averageBrightness.HasValue)
            {
                //brightness = args.lightEstimation.averageBrightness.Value;
                light.intensity = args.lightEstimation.averageBrightness.Value;
            }
            else
            {
                brightness = null;
            }

            if (args.lightEstimation.averageColorTemperature.HasValue)
            {
                colorTemperature = args.lightEstimation.averageColorTemperature.Value;
                light.colorTemperature = colorTemperature.Value;
            }
            else
            {
                colorTemperature = null;
            }

            if (args.lightEstimation.colorCorrection.HasValue)
            {
                colorCorrection = args.lightEstimation.colorCorrection.Value;
                light.color = colorCorrection.Value;
            }
            else
            {
                colorCorrection = null;
            }
    }
}
