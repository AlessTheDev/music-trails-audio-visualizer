using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static readonly int NUM_SAMPLES = 1024;

    [Header("Object References")]
    [SerializeField] private LoadingScreen loadingScreen;

    [Header("AudioSettings")]
    [SerializeField] private AudioClip clip;
    [SerializeField] private float _valuesMultiplier = 100;
    [SerializeField] private AudioSpectrum _earlyAudioSpectrum;
    [SerializeField] private AudioSpectrum _realtimeAudioSpectrum;
    [SerializeField] private AudioOutput _realtimeAudioOutput;
    [SerializeField] private float _delay;

    public float ValuesMultiplier => _valuesMultiplier;
    public AudioSpectrum EarlyAudioSpectrum => _earlyAudioSpectrum;
    public AudioSpectrum RealtimeAudioSpectrum => _realtimeAudioSpectrum;
    public AudioOutput RealtimeAudioOutput => _realtimeAudioOutput;
    public float Delay => _delay;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = -1; 
        if (Instance != null)
        {
            Debug.LogWarning("Can't have multiple instances of " + nameof(GameManager));
            Destroy(gameObject);
        }

        Instance = this;
        IsPaused = false;

        Debug.Log(Application.targetFrameRate);
    }

    private void Start()
    {
        if (AudioLoader.Instance == null)
        {
            StartCoroutine(SetupAudioSources());
        }
    }

    public void LoadClip(AudioClip clip, string songName)
    {
        _earlyAudioSpectrum.GetComponent<AudioSource>().Stop();
        _realtimeAudioSpectrum.GetComponent<AudioSource>().Stop();
        this.clip = clip;
        StartCoroutine(SetupAudioSources());
        StartCoroutine(loadingScreen.ShowLoadingSong(songName));
    }

    private IEnumerator SetupAudioSources()
    {
        AudioSource earlyAudioSource = _earlyAudioSpectrum.GetComponent<AudioSource>();
        AudioSource realtimeAudioSource = _realtimeAudioSpectrum.GetComponent<AudioSource>();

        earlyAudioSource.clip = clip;
        earlyAudioSource.Play();

        yield return new WaitForSecondsRealtime(_delay);

        realtimeAudioSource.clip = clip;
        realtimeAudioSource.Play();
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            _earlyAudioSpectrum.GetComponent<AudioSource>().Pause();
            _realtimeAudioSpectrum.GetComponent<AudioSource>().Pause();
        }
        else
        {
            _earlyAudioSpectrum.GetComponent<AudioSource>().Play();
            _realtimeAudioSpectrum.GetComponent<AudioSource>().Play();
        }
    }
}
