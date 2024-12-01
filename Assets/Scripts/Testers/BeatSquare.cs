using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BeatSquare : MonoBehaviour
{
    [SerializeField] private Color32 highlightedColor;
    [SerializeField] private Color32 normalColor;

    [SerializeField] private float toNormalTransitionSpeed;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnBeat()
    {
        image.color = highlightedColor;
    }

    private void Update()
    {
        image.color = Color32.Lerp(image.color, normalColor, toNormalTransitionSpeed * Time.deltaTime);
    }
}
