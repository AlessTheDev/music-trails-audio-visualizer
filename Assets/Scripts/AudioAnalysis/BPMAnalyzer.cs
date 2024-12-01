using UnityEngine;

public class BPMAnalyzer : MonoBehaviour
{
    [SerializeField] private BeatDetection beatDetectionAlgorythm;
    [SerializeField] private float interval;

    public int BPMs { get; private set; }   

    private float timer = 0;

    private int beatCount;

    private void Start()
    {
        beatDetectionAlgorythm.OnBeat.AddListener(AddBeatToCount);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > interval)
        {
            BPMs = (int)Mathf.Floor(60f / interval * beatCount);
            beatCount = 0;
            timer = 0;
        }
    }

    private void AddBeatToCount()
    {
        beatCount++;
    }
}
