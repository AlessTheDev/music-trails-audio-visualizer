using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScaling : MonoBehaviour
{
    [SerializeField] private BeatDetection beatDetector;
    [SerializeField] private float decreaseFactor;
    [SerializeField] private float smoothSpeed = 10;
    private float value;

    private float minScale;
    [SerializeField] private float maxScaleIncrease;

    private void Start()
    {
        minScale = transform.localScale.x;
        value = minScale;
        beatDetector.OnBeat.AddListener(OnBeat);
    }

    private void Update()
    {
        if(value > minScale)
        {
            value -= (decreaseFactor * Time.deltaTime);
        }
        transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, value, smoothSpeed * Time.deltaTime);
    }

    public void OnBeat()
    {
        value = minScale + maxScaleIncrease;
    }
}
