using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class LightsWindow : MonoBehaviour
{
    public Light2D lightFlamme;
    private float graphValue;
    public AnimationCurve curveIntensity;
    public float playbackSpeed = 0;

    void Update()
    {
        graphValue = curveIntensity.Evaluate(Time.time / playbackSpeed);
        lightFlamme.intensity = graphValue;
    }
}