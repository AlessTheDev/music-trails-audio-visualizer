using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LineArrayRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    [SerializeField] private float pointsDistance;
    [SerializeField] private float lerpSpeed = 10;
    [SerializeField] private float multiplier = 0.1f;

    public float[] data = new float[]{};

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = data.Length;
        for (int i = 0; i < data.Length; i++)
        {
            Vector3 initialPos = new Vector2(i * pointsDistance, data[i] * multiplier);
            lineRenderer.SetPosition(i, initialPos);
        }
    }

    private void Update()
    {
        lineRenderer.positionCount = data.Length;

        for(int i = 0; i < data.Length; i++)
        {
            Vector3 positionValue = new Vector2(i * pointsDistance, data[i] * multiplier);
            Vector3 newPos = transform.position + positionValue;
            lineRenderer.SetPosition(i, Vector3.Lerp(lineRenderer.GetPosition(i), newPos, Time.deltaTime * lerpSpeed));
        }
    }
}
