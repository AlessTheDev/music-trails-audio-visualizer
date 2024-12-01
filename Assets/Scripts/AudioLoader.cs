using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AudioLoader : MonoBehaviour
{
    public static AudioLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Can't have multiple instances of " + nameof(GameManager));    
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FileExplorerManager.Instance.Hide();
        FileExplorerManager.Instance.Show();
    }

    public void LoadSongToVisualizer()
    {
        FileVisual selectedFile = FileExplorerManager.Instance.selectedFile;
        string filePath = selectedFile.filePath + Path.DirectorySeparatorChar + selectedFile.fileName;
        Debug.Log("Loading: " + filePath);
        StartCoroutine(LoadSongToVisualizerCoroutine(filePath));
    }

    private IEnumerator LoadSongToVisualizerCoroutine(string filePath)
    {
        // Detect the audio type based on the file extension
        AudioType audioType = GetAudioType(filePath);

        string formattedPath = "file://" + filePath;

        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(formattedPath, audioType);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);

            SceneManager.LoadScene("CoolAudioVisualizer");
            yield return new WaitUntil(() => GameManager.Instance != null);
            GameManager.Instance.LoadClip(
                audioClip, 
                Path.GetFileNameWithoutExtension(filePath)
            );
        }
    }

    private AudioType GetAudioType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".mp3":
                return AudioType.MPEG;
            case ".wav":
                return AudioType.WAV;
            case ".ogg":
                return AudioType.OGGVORBIS;
            case ".aiff":
                return AudioType.AIFF;
            default:
                Debug.LogWarning("Unsupported audio type. Defaulting to WAV.");
                return AudioType.WAV;
        }
    }
}

