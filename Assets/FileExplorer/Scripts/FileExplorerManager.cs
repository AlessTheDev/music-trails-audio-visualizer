using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;
using System.Linq;
using UnityEngine.Events;

public class FileExplorerManager : MonoBehaviour
{
    public static FileExplorerManager Instance { get; private set; }

    public FileVisual selectedFile { get; private set; }

    private const string PLAYER_PREFS_KEY = "file-explorer-path";

    [SerializeField] private UnityEvent OnFileSelected;

    [Header("File Explorer Settings")]
    [SerializeField] private GameObject fileExplorerCanvas;
    [SerializeField] private GameObject fileList;
    [SerializeField] private TMP_InputField pathInputField;
    [SerializeField] private string[] extensionsFilter; //Leave blank to allow all extensions

    [Header("Visual Settings")]
    [SerializeField] private GameObject fileVisualPrefab;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private bool canClose = true;

    //Start path
    private string currentPath = $"{Application.dataPath}/../";

    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey(PLAYER_PREFS_KEY))
        {
            UpdatePath(PlayerPrefs.GetString(PLAYER_PREFS_KEY));
        }
    }

    /// <summary>
    /// Shows the FileExplorer
    /// </summary>
    public void Show()
    {
        fileExplorerCanvas.SetActive(true);
        closeButton.SetActive(canClose);

        pathInputField.text = currentPath;
        ShowFilesAndDirectories();
    }
    /// <summary>
    /// Hides the file explorer
    /// </summary>
    public void Hide()
    {
        fileExplorerCanvas.SetActive(false);
    }

    /// <summary>
    /// Shows all Directories and files in the file explorer depending on the current path
    /// </summary>
    public void ShowFilesAndDirectories()
    {
        // Removes old Files and Directories
        for (int i = fileList.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(fileList.transform.GetChild(i).gameObject);
        }

        //Adds new Directories
        DirectoryInfo[] directories = GetDirectory(currentPath);
        foreach (DirectoryInfo dir in directories)
        {
            GameObject fileVisual = Instantiate(fileVisualPrefab);
            fileVisual.transform.SetParent(fileList.transform);

            FileVisual fileVisualComponent = fileVisual.GetComponent<FileVisual>();
            fileVisualComponent.Initialize(dir.Name, dir.FullName, SupportedFileType.dir);
        }

        //Adds new files
        FileInfo[] files = GetFiles(currentPath);
        foreach (FileInfo file in files)
        {
            GameObject fileVisual = Instantiate(fileVisualPrefab);
            fileVisual.transform.SetParent(fileList.transform);

            FileVisual fileVisualComponent = fileVisual.GetComponent<FileVisual>();
            fileVisualComponent.Initialize(file.Name, file.Directory.FullName, FileVisual.GetTypeByExtension(file.Extension));
        }
    }

    /// <summary>
    /// Changes the path the explorer is currently in
    /// </summary>
    /// <param name="path"></param>
    public void UpdatePath(string path)
    {
        //Makes sure all \ are replaced with /
        string newPath = path.Replace("\\", "/");

        //Doesn't update the directory if it can't access it
        if (!CanAccessDirectory(newPath)) { return; }

        currentPath = newPath;
        pathInputField.text = currentPath;

        ShowFilesAndDirectories();
    }
    /// <summary>
    /// Changes the path the explorer is currently with the inputfield text
    /// </summary>
    public void UpdatePath()
    {
        string newPath = pathInputField.text;
        //Makes sure all \ are replaced with /
        newPath = newPath.Replace("\\", "/");

        //Removes the final / if necessary like C:/Documents/
        if (newPath.EndsWith("/") && newPath.Length != 3)
        {
            newPath = newPath.Substring(newPath.Length - 1);
        }

        //Doesn't update the directory if it can't access it
        if (!CanAccessDirectory(newPath)) { return; }

        currentPath = newPath;

        ShowFilesAndDirectories();
    }

    /// <summary>
    /// Goes to the parent directory
    /// </summary>
    public void GoBackDirectory()
    {
        if (currentPath.Contains("/"))
        {
            UpdatePath(currentPath.Substring(0, currentPath.LastIndexOf("/")));
            if (currentPath.Length == 2)
            {
                UpdatePath(currentPath + "/");
            }
        }
    }

    /// <summary>
    /// Checks if the Directory is accessable and if it exists
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool CanAccessDirectory(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists) { return false; }
        try
        {
            dir.GetFiles();
        }
        catch (System.UnauthorizedAccessException)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Opens a file
    /// </summary>
    /// <param name="file"></param>
    public void OpenFile(FileVisual file)
    {
        if (file.isDirectory)
        {
            UpdatePath(file.filePath);
        }
        else
        {
            selectedFile = file;
            PlayerPrefs.SetString(PLAYER_PREFS_KEY, file.filePath);
            OnFileSelected?.Invoke();
        }
    }

    public FileVisual GetSelectedFile()
    {
        return selectedFile;
    }

    /// <summary>
    /// Gets all the files in a path
    /// </summary>
    /// <param name="path"></param>
    /// <returns>An array containing all the files in a path
    private FileInfo[] GetFiles(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] files = dir.GetFiles("*.*");

        List<FileInfo> validFiles = new List<FileInfo>();

        //Return all files if there are no filters
        if (extensionsFilter.Length == 0) { return files; }
        foreach (FileInfo file in files)
        {
            if (extensionsFilter.Contains(file.Extension)) { validFiles.Add(file); }
        }

        return validFiles.ToArray();
    }
    /// <summary>
    /// Gets all the directories in a path
    /// </summary>
    /// <param name="path"></param>
    /// <returns>An array containing all the directories in a path
    private DirectoryInfo[] GetDirectory(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        DirectoryInfo[] directories = dir.GetDirectories();
        return directories;
    }
}
