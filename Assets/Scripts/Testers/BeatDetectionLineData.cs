using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineArrayRenderer))]
public class BeatDetectionLineData : MonoBehaviour
{
    private enum DataName
    {
        SpectralFlux,
        SmoothedSpectralFlux
    }
    
    [SerializeField] private BeatDetection beatDetectionAlgorithm;
    [SerializeField] private DataName fieldToGrab;
    
    private LineArrayRenderer rendererReference;
    private void Start()
    {
        rendererReference = GetComponent<LineArrayRenderer>();
    }

    private void Update()
    {
        float[] data = fieldToGrab switch
        {
            DataName.SpectralFlux => beatDetectionAlgorithm.SpectralFluxData,
            DataName.SmoothedSpectralFlux => beatDetectionAlgorithm.SmoothSpectralFluxData,
            _ => new float[]{}
        };

        if (data != null)
        {
            rendererReference.data = data;
        }
    }
}
