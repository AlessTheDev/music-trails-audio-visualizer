using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Shortcuts : MonoBehaviour
{
    [SerializeField] private Environment.SpecialFolder folder;
    public void UseShortcut()
    {
        UseShortcut(folder);
    }
    public void UseShortcut(Environment.SpecialFolder shortcut)
    {
        FileExplorerManager.Instance.UpdatePath(Environment.GetFolderPath(shortcut));
    }
}
