using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphDot : MonoBehaviour
{
    public float Time;
    public float Value;
    public string Name;

    public void ShowData()
    {
        GameObject.Find("DotStats").GetComponent<TextMeshProUGUI>().text = $"{Name} | Value: {Value} t: {Time} ";
    }
}
