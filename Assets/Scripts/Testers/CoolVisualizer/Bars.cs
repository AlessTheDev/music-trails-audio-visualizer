using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bars : MonoBehaviour
{
    private class BandBar
    {
        public BandBar(int startIndex, int endIndex)
        {
            this.startIndex = startIndex;
            this.endIndex = endIndex;
        }

        public Transform barTransform;
        public SpriteRenderer barRenderer;
        public float valueBuffer;
        public float maxValue;
        public float bufferDecrease;

        public int startIndex;
        public int endIndex;
    }

    [SerializeField] private int barsCount;
    [SerializeField] private Transform barPrefab;
    [SerializeField] private float gap;
    [SerializeField] private float bufferDecrease;
    [SerializeField] private float heightMultiplier = 0.4f;
    [SerializeField] private float barWidth = 1;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightedColor;

    private AudioSpectrum audioSpectrum;
    private BandBar[] bandBars;

    private float[] samples;
    private void Start()
    {
        audioSpectrum = GameManager.Instance.RealtimeAudioSpectrum;
        bandBars = new BandBar[barsCount];

        int indexJump = (GameManager.NUM_SAMPLES / 2) / barsCount;

        float offset = gap / 4 + barWidth / 2;
        int dir = 1;
        for (int i = 0; i < barsCount; i++)
        {
            int startIndex = i * indexJump;
            int endIndex = (i + 1) * indexJump;

            BandBar bar = new(startIndex, endIndex);
            bandBars[i] = bar;
            if (i <= 1)
            {
                CreateBar(offset * dir, bar);
            }
            else
            {
                CreateBar((offset + (barWidth / 2 + gap) * Mathf.Floor(i / 2)) * dir, bar);
            }

            dir *= -1;
        }
    }



    private void Update()
    {
        audioSpectrum.GetSamplesMultiplied(out samples);
        foreach (BandBar bar in bandBars)
        {
            UpdateBar(bar);
        }
    }

    private void CreateBar(float posX, BandBar data)
    {
        Transform bar = Instantiate(barPrefab, Vector2.zero, Quaternion.identity);
        bar.SetParent(transform);
        bar.localPosition = Vector2.right * posX;
        data.barTransform = bar;
        data.barRenderer = bar.GetComponent<SpriteRenderer>();
    }

    private void UpdateBar(BandBar b)
    {
        float value = CalclateBarValue(b);

        if(value > b.maxValue)
        {
            b.maxValue = value;
        }

        if (value > b.valueBuffer)
        {
            b.valueBuffer = value;
            b.bufferDecrease = 0.005f;
        }
        else if (value != b.valueBuffer)
        {
            b.valueBuffer -= b.bufferDecrease;
            b.bufferDecrease *= bufferDecrease;
        }

        Vector2 targetScale = new(
            barWidth,
            barWidth + b.valueBuffer * heightMultiplier
        );

        b.barTransform.localScale = targetScale;

        b.barRenderer.color = Color.Lerp(normalColor, highlightedColor, value / b.maxValue);
    }

    private float CalclateBarValue(BandBar bar)
    {
        float sum = 0;
        for (int i = bar.startIndex; i < bar.endIndex; i++)
        {
            sum += samples[i];
        }
        return sum / (bar.endIndex - bar.startIndex);
    }
}
