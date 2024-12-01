using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunnyHat : MonoBehaviour
{
    [SerializeField] private float speed360;
    [SerializeField] private float swingSpeed;
    [SerializeField] private float swingMaxRange;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementRange;

    private bool do360 = false;
    private float tmpAngle;
    private float angleProgress;

    private float swingT = 0;
    private float movementT = 0;

    private Vector2 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = startPos + Mathf.Sin(movementT) * movementRange * Vector2.one * Vector3.up;
        movementT += Time.deltaTime * movementSpeed * GameManager.Instance.RealtimeAudioSpectrum.GetFrequencyBandValue(FrequencyBandName.LowMidrange);
        if (do360)
        {
            Handle360();
        }
        else
        {
            HandleSwing();
        }
    }

    private void Handle360()
    {
        angleProgress = Mathf.Lerp(angleProgress, 360 + 30f, speed360 * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0, 0, tmpAngle + angleProgress);
        if (Mathf.Abs(angleProgress - 360) < 1f)
        {
            do360 = false;
        }
    }

    private void HandleSwing()
    {
        transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(swingT) * swingMaxRange);
        swingT += Time.deltaTime * swingSpeed;
    }

    public void OnBeat()
    {
        if (do360) return;
        do360 = true;
        tmpAngle = transform.localEulerAngles.z;
        angleProgress = 0;
    }
}
