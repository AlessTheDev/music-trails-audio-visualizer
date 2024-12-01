using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField] private BPMAnalyzer bpmAnalyzer;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float speedMultiplier = 1;

    private float timer = 0;
    private int sprite;

    private void Update()
    {
        timer += Time.deltaTime;
        //float bpmPower = Mathf.Pow(bpmAnalyzer.BPMs, 2);
        float bpmPower = bpmAnalyzer.BPMs;
        if(timer > Time.deltaTime / (speedMultiplier * bpmPower))
        {
            sprite++;

            if(sprite == sprites.Length) sprite = 0;
            timer = 0;

            spriteRenderer.sprite = sprites[sprite];
        }
    }
}
