using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileIcons : MonoBehaviour
{
    public static FileIcons Instance { get; private set; }

    [SerializeField] private Sprite directoryFileIcon;
    [SerializeField] private Sprite audioFileIcon;
    [SerializeField] private Sprite imageFileIcon;
    [SerializeField] private Sprite videoFileIcon;
    [SerializeField] private Sprite unknownFileIcon;
    void Awake()
    {
        Instance = this;
    }

    public Sprite GetFileIcon(SupportedFileType type)
    {
        switch (type)
        {
            case SupportedFileType.mp3:
                return audioFileIcon;
            case SupportedFileType.mp4:
                return videoFileIcon;
            case SupportedFileType.png:
            case SupportedFileType.jpg:
                return imageFileIcon;
            case SupportedFileType.dir:
                return directoryFileIcon;
            default: return unknownFileIcon;
        }
    }
}
