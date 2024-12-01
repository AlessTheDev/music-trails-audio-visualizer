using UnityEngine;

public class AverageGraph : Graph
{
    private GraphWrapper wrapper;

    private float timer = 0;

    private void Start()
    {
        BaseStart();
        wrapper = GraphWrapper.Instance;
        graphName = "Average";
    }
    private void Update()
    {
        if (timer > wrapper.updateInterval)
        {
            AddValue(wrapper.RealtimeAudioSpectrum.GetAverage()); 
            BaseUpdate();
            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
