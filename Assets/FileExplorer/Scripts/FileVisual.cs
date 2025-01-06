using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FileVisual : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;
    public string fileName { get; private set; }
    public string filePath { get; private set; }
    public bool isDirectory { get; private set; }

    public void Initialize(string fileName, string filePath, SupportedFileType fileType)
    {
        this.fileName = fileName;
        this.filePath = filePath;

        icon.sprite = FileIcons.Instance.GetFileIcon(fileType);
        text.text = fileName;
        isDirectory = fileType == SupportedFileType.dir; 

        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(607.25f, 30f);
    }

    public static SupportedFileType GetTypeByExtension(string extension)
    {
        switch(extension)
        {
            case ".mp3":
                return SupportedFileType.mp3;
            case ".mp4":
                return SupportedFileType.mp4;
            case ".png":
                return SupportedFileType.png;
            case ".jpg":
                return SupportedFileType.jpg;
            case ".jpeg":
                return SupportedFileType.jpeg;
            default:
                return SupportedFileType.noExtension;
        }
    }

    public void Click()
    {
        FileExplorerManager.Instance.OpenFile(this);
    }

}

