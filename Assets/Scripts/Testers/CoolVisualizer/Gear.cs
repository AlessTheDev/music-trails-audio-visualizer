using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] private bool isAverage;
    [SerializeField] private FrequencyBandName frequencyBand;
    [SerializeField] private float speedMultiplier = 1;

    private AudioSpectrum spectrum;
    private void Start()
    {
        spectrum = GameManager.Instance.RealtimeAudioSpectrum;
    }
    private void Update()
    {
        float value = isAverage
            ? spectrum.GetAverage()
            : spectrum.GetFrequencyBandValue(frequencyBand);
        transform.Rotate(Vector3.forward, value * speedMultiplier * Time.deltaTime * -1);
    }
}
