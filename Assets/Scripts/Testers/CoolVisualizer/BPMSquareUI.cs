using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BPMSquareUI : MonoBehaviour
{
    [SerializeField] private BPMAnalyzer bpmAnalyzer;

    [SerializeField] private Vector2 left;
    [SerializeField] private Vector2 right;

    private Vector2 target;
    private float elapsedTime = 0;
    private float timeToReachTarget;

    private RectTransform rt;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        target = left;
        CalculateTimeToReachTarget();
    }

    private void Update()
    {
        CalculateTimeToReachTarget();

        elapsedTime += Time.deltaTime;

        float t = elapsedTime / timeToReachTarget;

        rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, target, t);

        if (elapsedTime >= timeToReachTarget)
        {
            elapsedTime = 0;
            target = target == left ? right : left;
        }
    }

    private void CalculateTimeToReachTarget()
    {
        timeToReachTarget = 60f / bpmAnalyzer.BPMs;
    }
}
