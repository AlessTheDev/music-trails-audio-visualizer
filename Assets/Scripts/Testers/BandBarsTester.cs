using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class BandBarsTester : MonoBehaviour
{
    [System.Serializable]
    private class BandBar
    {
        public FrequencyBandName name;
        public Color32 color;
        public Transform barTransform;
        public SpriteRenderer barRenderer;
        public bool isAverage;
        public float valueBuffer;
        public float bufferDecrease;
    }


    [SerializeField] private AudioSpectrum audioSpectrum;
    [SerializeField] private Transform barPrefab;
    [SerializeField] private TextMeshProUGUI legend;
    [SerializeField] private float gap;
    [SerializeField] private float scaleSpeed;

    [Header("Buffer Settings")]
    [SerializeField] private bool useBuffers;
    [SerializeField] private float bufferDecrease;

    [SerializeField] private List<BandBar> frequencyBands = new() { new BandBar { isAverage = true, color = Color.white } };
    private void Start()
    {
        float startPos = barPrefab.localScale.x * (frequencyBands.Count + 1) / 2 * -1;
        for (int i = 0; i < frequencyBands.Count; i++)
        {
            CreateBar(startPos + (barPrefab.localScale.x / 2 + gap) * i, frequencyBands[i]);
        }

        foreach (BandBar bar in frequencyBands)
        {
            legend.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(bar.color)}>{(bar.isAverage ? "Average" : bar.name)}</color>\n";
            legend.richText = true;
        }
    }

    private void CreateBar(float posX, BandBar data)
    {
        Transform bar = Instantiate(barPrefab, Vector2.right * posX, Quaternion.identity);
        bar.SetParent(transform);
        data.barTransform = bar;
        data.barRenderer = bar.GetComponent<SpriteRenderer>();
        data.barRenderer.color = data.color;
    }


    private void Update()
    {
        foreach (BandBar bar in frequencyBands)
        {
            UpdateBar(bar);
        }
    }

    private void UpdateBar(BandBar b)
    {
        float value = b.isAverage
                    ? audioSpectrum.GetAverage()
                    : audioSpectrum.GetFrequencyBandValue(b.name);

        if(value > b.valueBuffer)
        {
            b.valueBuffer = value;
            b.bufferDecrease = 0.005f;
        }
        else if(value != b.valueBuffer)
        {
            b.valueBuffer -= b.bufferDecrease;
            b.bufferDecrease *= bufferDecrease;
        }
        Vector2 targetScale = new Vector2(
                b.barTransform.localScale.x,
                (useBuffers ? b.valueBuffer : value) / 1.4f
           );

        b.barTransform.localScale = targetScale;

        b.barRenderer.color = Color.Lerp(GetDarkerColor(b.color, 0.5f), GetWhiterColor(b.color, 0.5f), audioSpectrum.GetFrequencyRangedValue(b.name));
    }

    private Color GetDarkerColor(Color originalColor, float lerpFactor)
    {
        return Color.Lerp(originalColor, Color.black, lerpFactor);
    }

    private Color GetWhiterColor(Color originalColor, float lerpFactor)
    {
        return Color.Lerp(originalColor, Color.white, lerpFactor);
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(BandBarsTester))]
    class BandsTesterEditor : Editor
    {
        private FrequencyBandName bandToAdd = FrequencyBandName.SubBass;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BandBarsTester bandsTester = (BandBarsTester)target;
            if (target == null) return;


            EditorGUILayout.Space();

            for (int i = 0; i < bandsTester.frequencyBands.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                BandBar band = bandsTester.frequencyBands[i];

                EditorGUILayout.LabelField(band.isAverage ? "Average" : band.name.ToString());
                band.color = EditorGUILayout.ColorField(band.color);

                bandsTester.frequencyBands[i] = band;

                if (GUILayout.Button("-"))
                {
                    bandsTester.frequencyBands.RemoveAt(i);
                    i--;
                }

                EditorGUILayout.EndHorizontal();
            }



            EditorGUILayout.BeginHorizontal();
            bandToAdd = (FrequencyBandName)EditorGUILayout.EnumPopup(bandToAdd);
            if (GUILayout.Button("Add Band"))
            {
                bandsTester.frequencyBands.Add(new BandBar { isAverage = false, name = bandToAdd, color = Color.white });
            }
            EditorGUILayout.EndHorizontal();

            EditorUtility.SetDirty(bandsTester);


        }
    }

#endif
}
