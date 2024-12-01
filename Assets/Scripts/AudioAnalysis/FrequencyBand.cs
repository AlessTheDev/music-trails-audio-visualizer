public class FrequencyBand
{
    public int MinFrequency { get; private set; }
    public int MaxFrequency { get; private set; }
    public FrequencyBandName Name { get; private set; }
    public float Value { get; set; }

    public FrequencyBand(FrequencyBandName name, int minFrequancy, int maxFrequancy)
    {
        MinFrequency = minFrequancy;
        MaxFrequency = maxFrequancy;
        Name = name;
        Value = 0;
    }
}
