using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private bool takeAverage;
    [SerializeField] private FrequencyBandName bandMultiplier = FrequencyBandName.Midrange;

    [Header("X Movement Settings")]
    [SerializeField] private float xSpeed;
    [SerializeField] private float xRange;

    [Header("Y Movement Settings")]
    [SerializeField] private float ySpeed;
    [SerializeField] private float yRange;

    private Camera cam;
    private AudioSpectrum audioSpectrum;

    private Vector3 targetPos;

    private float xT;
    private float yT;

    private void Start()
    {
        cam = GetComponent<Camera>();
        audioSpectrum = GameManager.Instance.RealtimeAudioSpectrum;
    }

    private void Update()
    {
        targetPos = new Vector3(Mathf.Cos(xT) * xRange, Mathf.Sin(yT) * yRange, transform.localPosition.z);
        transform.localPosition = targetPos;

        float value = takeAverage
            ? audioSpectrum.GetAverage()
            : audioSpectrum.GetFrequencyBandValue(bandMultiplier);
        xT += Time.deltaTime * xSpeed * value;
        yT += Time.deltaTime * ySpeed * value;
    }
}
