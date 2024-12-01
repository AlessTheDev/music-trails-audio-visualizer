using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPMSquare : MonoBehaviour
{
    [SerializeField] private BPMAnalyzer bpmAnalyzer;
    [SerializeField] private Transform leftTarget;
    [SerializeField] private Transform rightTarget;

    private Transform target;
    private float elapsedTime = 0;
    private float timeToReachTarget;

    private void Start()
    {
        target = leftTarget;
        CalculateTimeToReachTarget();
    }

    private void Update()
    {
        CalculateTimeToReachTarget();

        elapsedTime += Time.deltaTime;

        float t = elapsedTime / timeToReachTarget;

        transform.position = Vector3.Lerp(transform.position, target.position, t);

        if (elapsedTime >= timeToReachTarget)
        {
            elapsedTime = 0;
            target = target == leftTarget ? rightTarget : leftTarget;
        }
    }

    private void CalculateTimeToReachTarget()
    {
        timeToReachTarget = 60f / bpmAnalyzer.BPMs;
    }
}
