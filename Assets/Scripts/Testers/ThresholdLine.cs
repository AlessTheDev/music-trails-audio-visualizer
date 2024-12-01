using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdLine : MonoBehaviour
{
    [SerializeField] private float multiplier = 0.1f;

    public float value;
    
    private void Update()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, value * multiplier);
    }
}
