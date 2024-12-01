using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutput : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public float[] Samples { get; private set; }  = new float[GameManager.NUM_SAMPLES];

    private void Update()
    {
        audioSource.GetOutputData(Samples, 0);  
    }
}
