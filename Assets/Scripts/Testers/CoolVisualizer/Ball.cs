using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] private FrequencyBandName band;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightedColor;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float rotationSpeed;

    private AudioSpectrum audioSpectrum;
    private float startY;

    private void Start()
    {
        audioSpectrum = GameManager.Instance.RealtimeAudioSpectrum;
        startY = transform.localPosition.y;
    }

    private void Update()
    {
        float value = audioSpectrum.GetFrequencyRangedValue(band);

        Vector2 targetPos = new Vector2(transform.localPosition.x, startY + value * range);
        spriteRenderer.color = Color.Lerp(normalColor, highlightedColor, value);

        transform.localPosition = Vector2.Lerp(transform.localPosition, targetPos, speed * Time.deltaTime);

        transform.Rotate(Vector3.forward * (rotationSpeed * audioSpectrum.GetFrequencyBandValue(band) * Time.deltaTime));
    }
}
