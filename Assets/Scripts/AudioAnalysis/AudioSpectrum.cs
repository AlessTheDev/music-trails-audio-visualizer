using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSpectrum : MonoBehaviour
{
    private AudioSource audioSource;
    private GameManager gameManager;

    // Spectrum values
    public int SAMPLE_RATE { get; private set; }
    private float[] samples = new float[GameManager.NUM_SAMPLES];

    private Dictionary<FrequencyBandName, FrequencyBand> frequencies = new();

    private Dictionary<FrequencyBandName, float> frequenciesHighest = new(); // Used to calculate the ranged values

    // Calculated values
    private float frequenciesSum;

    private float songAverageSum;
    private int updateCount = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SAMPLE_RATE = AudioSettings.outputSampleRate / 2;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        AddFrequencyBand(FrequencyBandName.SubBass, 20, 60);
        AddFrequencyBand(FrequencyBandName.Bass, 60, 250);
        AddFrequencyBand(FrequencyBandName.LowMidrange, 250, 500);
        AddFrequencyBand(FrequencyBandName.Midrange, 500, 2000);
        AddFrequencyBand(FrequencyBandName.UpperMidrange, 2000, 4000);
        AddFrequencyBand(FrequencyBandName.High, 4000, 6000);
        AddFrequencyBand(FrequencyBandName.Brilliance, 6000, 20000);
        AddFrequencyBand(FrequencyBandName.Snare, 100, 2500);
        AddFrequencyBand(FrequencyBandName.Piano, 250, 2000);
    }

    private void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Hanning);
        UpdateFrequenciesValues();
    }

    private void AddFrequencyBand(FrequencyBandName bandName, int minFrequency, int maxFrequency)
    {
        frequencies.Add(bandName, new FrequencyBand(bandName, minFrequency, maxFrequency));
        frequenciesHighest.Add(bandName, 0);
    }

    private void UpdateFrequenciesValues()
    {
        frequenciesSum = 0;

        // Frequencies Sum
        for (int i = 0; i < GameManager.NUM_SAMPLES; i++)
        {
            frequenciesSum += samples[i] * gameManager.ValuesMultiplier; 
        }

        songAverageSum += GetAverage();
        updateCount++;

        // Calculate 
        foreach (FrequencyBandName frequencyBandName in frequencies.Keys)
        {
            FrequencyBand frequencyBand = frequencies[frequencyBandName];

            // Convert the sample to an index of the array
            int startSampleIndex = GetSampleIndex(frequencyBand.MinFrequency);
            int endSampleIndex = GetSampleIndex(frequencyBand.MaxFrequency);

            int diff = endSampleIndex - startSampleIndex;

            // Sum all values and calculate the average
            float sum = 0;
            for (int i = startSampleIndex; i < endSampleIndex; i++)
            {
                sum += samples[i] * gameManager.ValuesMultiplier;
            }

            frequencyBand.Value = sum / diff;

            if(frequencyBand.Value > frequenciesHighest[frequencyBandName])
            {
                frequenciesHighest[frequencyBandName] = frequencyBand.Value;
            }
        }
    }

    public int GetSampleIndex(float frequency)
    {
        float HzPerSample = SAMPLE_RATE / samples.Length;
        return Mathf.Max(0, (int)(frequency / HzPerSample) - 1);
    }

    public float GetFrequencyBandValue(FrequencyBandName frequencyBand)
    {
        return frequencies[frequencyBand].Value;
    }
    public float GetFrequencyRangedValue(FrequencyBandName frequencyBand)
    {
        if (frequenciesHighest[frequencyBand] == 0) return 0;
        return frequencies[frequencyBand].Value / frequenciesHighest[frequencyBand];
    }

    public float GetAverage()
    {
        return frequenciesSum / GameManager.NUM_SAMPLES;
    }

    public float GetSongAverage()
    {
        return songAverageSum / updateCount;
    }

    /// <summary>
    /// Returns the samples multiplied by the GameManager.valuesMultiplier
    /// </summary>
    /// <param name="samples">The output array</param>
    public void GetSamplesMultiplied(out float[] samples)
    {
        samples = new float[this.samples.Length];
        for (int i = 0; i < this.samples.Length; i++)
        {
            samples[i] = this.samples[i] * gameManager.ValuesMultiplier;
        }
    }

    /// <summary>
    /// Returns the samples within a specified range multiplied by the GameManager.valuesMultiplier
    /// </summary>
    /// <param name="samples">The output array</param>
    /// <param name="band">The band of samples</param>
    public void GetSamplesMultiplied(out float[] samples, FrequencyBandName band)
    {
        int i = frequencies[band].MinFrequency;
        int j = frequencies[band].MaxFrequency;

        float[] tempSamples = this.samples.Skip(i).Take(j - i + 1).ToArray();
        samples = new float[tempSamples.Length];
        for (int k = 0; k < tempSamples.Length; k++)
        {
            samples[k] = tempSamples[k] * gameManager.ValuesMultiplier;
        }
    }

   

}
