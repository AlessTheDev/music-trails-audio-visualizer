using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AudioOutputVisualizer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private AudioOutput audioOutput;

    [SerializeField] private float pointsDistance;

    [SerializeField] private float lerpSpeed = 10;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = GameManager.NUM_SAMPLES;
        audioOutput = GameManager.Instance.RealtimeAudioOutput;
    }
    private void Update()
    {
        for(int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 positionValue = new Vector2(i * pointsDistance, audioOutput.Samples[i]);
            Vector3 newPos = transform.position + positionValue;
            lineRenderer.SetPosition(i, Vector3.Lerp(lineRenderer.GetPosition(i), newPos, Time.deltaTime * lerpSpeed));
        }
    }
}
