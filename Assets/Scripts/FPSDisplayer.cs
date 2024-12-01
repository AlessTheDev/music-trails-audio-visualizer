using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    [SerializeField] private float pollingTime = 1f;
    private float time;
    private float frameCount;

    private void Update()
    {
        time += Time.deltaTime;

        frameCount++;

        if(time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = $"FPS: {frameRate}";

            time -= pollingTime;
            frameCount = 0;
        }
    }

}
