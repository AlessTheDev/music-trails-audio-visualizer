using System.Collections.Generic;
using UnityEngine;

public class GraphWrapper : MonoBehaviour
{
    public static GraphWrapper Instance { get; private set; }

    [Header("Graph Settings")]
    public RectTransform graphContainer;
    public float unitDistance;
    public float movementSpeed;
    public float updateInterval;
    public bool deleteOutsideobjects;


    [Header("Dot Settings")]
    public RectTransform dotPrefab;
    public float dotSize;

    // GameManager references 
    private GameManager gameManager;

    public AudioSpectrum EarlyAudioSpectrum => gameManager.EarlyAudioSpectrum;
    public AudioSpectrum RealtimeAudioSpectrum => gameManager.RealtimeAudioSpectrum;


    // Keep track of max values
    public Dictionary<string, float> MaxValues { get; private set; } = new Dictionary<string, float>();
    public float MaxValue { get; private set; } // The max values of the bands I'm taking in consideration

    private string lastFocusAuthor; // Keeps track of the last band that modified MaxValue
    public List<string> focusList;
    private int previousListCount = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Can't have 2 instances of " + GetType().Name);
            Destroy(gameObject);
        }
        Debug.Log("Wrapper Instance");
        Instance = this;
        previousListCount = focusList.Count;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void TrySetMaxValue(string band, float value)
    {
        if (focusList.Count != previousListCount)
        {
            MaxValues.Clear();
            MaxValue = value;
            previousListCount = focusList.Count;
        }
        if (!MaxValues.ContainsKey(band)) { MaxValues.Add(band, value); return; }
        
        if (!focusList.Contains(lastFocusAuthor) && focusList.Contains(band))
        {
            lastFocusAuthor = band;
            MaxValue = value;
        }
        if (value > MaxValues[band])
        {
            MaxValues[band] = value;
            
            if (focusList.Count == 0 || focusList.Contains(band))
            {
                if (value > MaxValue)
                {
                    lastFocusAuthor = band;
                    MaxValue = value;
                }
            }
        }
    }

    public void TogglePause()
    {
        gameManager.TogglePause();
    }
}
