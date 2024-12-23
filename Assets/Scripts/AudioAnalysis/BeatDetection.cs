using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpectralFluxData
{
    public float value;
    public float time;
}

public class BeatDetection : MonoBehaviour
{
    [SerializeField] private AudioSpectrum audioSpectrum;

    [Header("Beat Detection Settings")] [SerializeField]
    private int spectralWindowSize = 200; // The spectralFlux values included to calculate the threshold

    [SerializeField] private int smoothingKernelSize = 4; // The kernel size ex [1/4, 1/4, 1/4, 1/4] 
    [SerializeField] private float thresholdMultiplier = 9.2f;
    [SerializeField] private float minStandardDeviation = 30;
    [SerializeField] private float minBeatTimeDistance = 0.08f;

    [Header("Band Settings")]
    // Analyzes all the samples, if it's false, it will analyze only the samples within the frequency band
    [SerializeField]
    private bool analyzeGlobal;

    [SerializeField] private FrequencyBandName frequencyBand;

    private float[] previousSpectrum;
    private float[] currentSpectrum;

    private List<SpectralFluxData> spectralFluxList = new();
    public bool isListFilled { get; private set; } = false;

    public UnityEvent OnBeat;
    private float lastBeatTime;
    
    // Info Fields
    public float[] SpectralFluxData { get; private set; }
    public float[] SmoothSpectralFluxData { get; private set; }
    public float Threshold { get; private set; }

    private void FixedUpdate()
    {
        // Get the samples
        if (analyzeGlobal)
        {
            audioSpectrum.GetSamplesMultiplied(out currentSpectrum);
        }
        else
        {
            audioSpectrum.GetSamplesMultiplied(out currentSpectrum, frequencyBand);
        }

        if (previousSpectrum == null)
        {
            previousSpectrum = new float[currentSpectrum.Length];
            currentSpectrum.CopyTo(previousSpectrum, 0);
            return;
        }

        SpectralFluxData spectralFlux = new()
        {
            value = GetSpectralFlux(),
            time = Time.time
        };
        spectralFluxList.Add(spectralFlux);

        // Make sure the window stays at a fixed size
        if (spectralFluxList.Count > spectralWindowSize)
        {
            isListFilled = true;

            spectralFluxList.RemoveAt(0);
            if (IsBeat(spectralFlux) && (Time.time - lastBeatTime >= minBeatTimeDistance))
            {
                lastBeatTime = Time.time;
                StartCoroutine(NotifyOnBeat(spectralFlux));
            }
        }

        // Update the previous spectrum
        currentSpectrum.CopyTo(previousSpectrum, 0);
    }

    /// <summary>
    /// The rectified change in the power spectrum of the audio signal over time
    /// </summary>
    /// <returns>The Spectral Flux</returns>
    private float GetSpectralFlux()
    {
        float sum = 0;
        for (int i = 0; i < currentSpectrum.Length; i++)
        {
            float rectifiedValue = Mathf.Max(0, currentSpectrum[i] - previousSpectrum[i]);
            sum += Mathf.Pow(rectifiedValue, 2);
        }

        return sum;
    }

    /// <summary>
    /// Checks if a specified Spectral Flux value is higher than the threshold
    /// </summary>
    /// <param name="sf">The Spectral Flux value</param>
    /// <returns>
    /// true if value is higher than the threshold,
    /// false if value is lower than the threshold
    /// </returns>
    private bool IsBeat(SpectralFluxData sf)
    {
        float[] spectralFluxValues = new float[spectralFluxList.Count];
        for (int i = 0; i < spectralFluxList.Count; i++)
        {
            spectralFluxValues[i] = spectralFluxList[i].value;
        }
        

        float kernelValues = 1 / smoothingKernelSize;
        float[] kernel = new float[smoothingKernelSize];
        for (int i = 0; i < smoothingKernelSize; i++)
        {
            kernel[i] = kernelValues;
        }

        float[] smoothedSpectralFlux = Convolve(spectralFluxValues, smoothingKernelSize);
        
        float mean = CalculateAverage(smoothedSpectralFlux);
        float standardDeviation = CalculateStandardDeviation(smoothedSpectralFlux, mean);

        float threshold = CalculateThreshold(mean, standardDeviation);
        
        // Update info fields
        SpectralFluxData = spectralFluxValues; 
        SmoothSpectralFluxData = smoothedSpectralFlux; 
        Threshold = threshold;

        return sf.value > threshold && standardDeviation > minStandardDeviation;
    }
    
    /// <summary>
    /// Calculates the threshold using the Standard Deviation
    /// </summary>
    /// <param name="smoothedSpectralFlux">The convolved Spectral Flux values</param>
    /// <returns>The threshold</returns>
    private float CalculateThreshold(float mean, float standardDeviation)
    {
        return mean + standardDeviation * thresholdMultiplier;
    }

    private float CalculateAverage(float[] values)
    {
        float sum = 0;
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }

        return sum / values.Length;
    }

    private float CalculateStandardDeviation(float[] values, float mean)
    {
        float varianceSum = 0;
        for (int i = 0; i < values.Length; i++)
        {
            varianceSum += Mathf.Pow(values[i] - mean, 2);
        }

        return Mathf.Sqrt(varianceSum / values.Length);
    }

    private IEnumerator NotifyOnBeat(SpectralFluxData spectralFluxData)
    {
        yield return new WaitForSecondsRealtime(spectralFluxData.time + GameManager.Instance.Delay - Time.time);
        OnBeat?.Invoke();
    }

    private float[] Convolve(float[] signal, int kernelSize)
    {
        // Create the kernel
        float[] kernel = new float[kernelSize];
        for (int i = 0; i < kernelSize; i++)
        {
            kernel[i] = 1.0f / kernelSize;
        }

        int signalLength = signal.Length;
        int outputLength = signalLength + kernelSize - 1;
        float[] output = new float[outputLength];

        for (int i = 0; i < outputLength; i++)
        {
            output[i] = 0;
            for (int j = 0; j < kernelSize; j++)
            {
                if (i - j >= 0 && i - j < signalLength)
                {
                    output[i] += signal[i - j] * kernel[j];
                }
            }
        }

        // Truncate the output to match the input length
        float[] truncatedOutput = new float[signalLength];
        for (int i = 0; i < signalLength; i++)
        {
            truncatedOutput[i] = output[i];
        }

        return truncatedOutput;
    }
}