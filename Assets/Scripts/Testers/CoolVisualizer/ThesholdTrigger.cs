using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ThesholdTrigger : MonoBehaviour
{
    [SerializeField] private FrequencyBandName bandName;
    [SerializeField] private float threshold;

    public UnityEvent OnTrigger;

    private AudioSpectrum audioSpectrum;

    private void Update()
    {
        if(audioSpectrum.GetFrequencyBandValue(bandName) >= threshold)
        {
            OnTrigger?.Invoke();
        }
    }
}
