using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private float threshold;

    [SerializeField] private float decreaseFactor;
    [SerializeField] private float smoothSpeed = 10;
    private float value;

    private float minSize;
    [SerializeField] private float maxZoomIncrease;

    private void Start()
    {
        cam = GetComponent<Camera>();

        minSize = cam.orthographicSize;
        value = minSize;
    }

    private void Update()
    {
        if (value < minSize)
        {
            value += (decreaseFactor * Time.deltaTime);
        }
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, value, smoothSpeed * Time.deltaTime);

        if(GameManager.Instance.RealtimeAudioSpectrum.GetFrequencyBandValue(FrequencyBandName.Bass) >= threshold)
        {
            OnBeat();
        }
    }

    public void OnBeat()
    {
        value = minSize - maxZoomIncrease;
    }
}
