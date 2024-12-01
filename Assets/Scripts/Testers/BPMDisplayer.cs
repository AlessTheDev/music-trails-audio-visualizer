using TMPro;
using UnityEngine;

public class BPMDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bpmText;
    [SerializeField] private BPMAnalyzer bpmAnalyzer;
    private void Update()
    {
        bpmText.text = $"BPM: {bpmAnalyzer.BPMs}";
    }

}
