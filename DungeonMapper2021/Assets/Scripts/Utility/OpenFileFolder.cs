using System;
using System.Diagnostics;
using UnityEngine;

public class OpenFileFolder : MonoBehaviour
{
    //public string folderPath;

    public void OpenSessionFolder()
    {

        string absolutePath = Session.SessionFolderDataPath;

        try
        {
            // For Windows
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = absolutePath,
                    UseShellExecute = true
                });
            }
            // For macOS
            else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
            {
                Process.Start("open", $"\"{absolutePath}\"");
            }
            // For Linux
            else if (Application.platform == RuntimePlatform.LinuxPlayer)
            {
                Process.Start("xdg-open", $"\"{absolutePath}\"");
            }
            else
            {
                UnityEngine.Debug.LogError("Platform not supported for opening folders.");
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Failed to open folder: {e.Message}");
        }
    }
}
