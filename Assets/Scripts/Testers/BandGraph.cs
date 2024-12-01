using UnityEngine;

public class BandGraph : Graph
{
    [SerializeField] private FrequencyBandName frequencyBand;
    private GraphWrapper wrapper;

    private float timer = 0;

    private void Start()
    {
        BaseStart();
        wrapper = GraphWrapper.Instance;
        graphName = frequencyBand.ToString();   
    }
    private void Update()
    {
        if (timer > wrapper.updateInterval)
        {
            AddValue(wrapper.RealtimeAudioSpectrum.GetFrequencyBandValue(frequencyBand));
            BaseUpdate();
            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
