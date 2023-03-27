using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class PreferencesData
{
    static string DataPath { get { return Application.persistentDataPath + "/Preferences" + SaveFileTypeName; } }
    private const string SaveFileTypeName = ".prefs";

    public const int minScrollSpeed = 1;
    public const int maxScrollSpeed = 100;


    public static bool allowedScrollSpeedCheck(int currentScrollSpeed)
    {
        if (currentScrollSpeed > minScrollSpeed && currentScrollSpeed < maxScrollSpeed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    int _camScrollSpeed = 20;
    public int camScrollSpeed
    {
        get
        {
            return Mathf.Clamp(_camScrollSpeed, minScrollSpeed, maxScrollSpeed);
        }
        set
        {
            _camScrollSpeed = Mathf.Clamp(value, minScrollSpeed, maxScrollSpeed);
        }
    }

    public const int minBrushSize = 10;
    public const int maxMaxBrushSize = 500;

    int _brushSize = 20;
    public int BrushSize { get { return Mathf.Clamp(_brushSize, minBrushSize, maxMaxBrushSize); } set {
            _brushSize = Mathf.Clamp(value, minBrushSize, maxMaxBrushSize);
        } 
    }

    public int BrushIndex = 0;

    public static void SavePreferences(PreferencesData data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(DataPath, FileMode.Create);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public static PreferencesData GetPreferences()
    {

        
        if (File.Exists(DataPath))
        {
            Debug.Log("Getting existing Preferences data");
            PreferencesData data = null;

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream stream = new FileStream(DataPath, FileMode.Open))
            {
                data = (PreferencesData)bf.Deserialize(stream) as PreferencesData;
                stream.Close();
            }

            return data;
        } else
        {
            Debug.Log("Generating new Preferences");

            PreferencesData data = new PreferencesData();
            SavePreferences(data);
            return data;
        }



    }


}
