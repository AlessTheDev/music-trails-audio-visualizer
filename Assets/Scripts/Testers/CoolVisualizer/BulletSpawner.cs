using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private FrequencyBandName speedBandReference;
    [SerializeField] private FrequencyBandName colorBandReference;

    public float SpeedMultiplier { get; private set; }
    public float RangedValue { get; private set; }

    private AudioSpectrum audioSpectrum;

    private void Start()
    {
        audioSpectrum = GameManager.Instance.RealtimeAudioSpectrum; 
    }

    private void Update()
    {
        SpeedMultiplier = audioSpectrum.GetFrequencyBandValue(speedBandReference);
        RangedValue = audioSpectrum.GetFrequencyRangedValue(colorBandReference);

        if(transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }

    public void OnBeat()
    {
        Bullet b1 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet b2 = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        b1.AssignSpawner(this);  
        b2.AssignSpawner(this);

        b2.SetDirection(-1);
    }
}
