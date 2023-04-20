using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePreferences : MonoBehaviour
{
    public void SaveCurrentPreferences()
    {
        Debug.Log("Saving prefs");
        PreferencesData.SavePreferences(StaticVariables.Preferences);
    }
    
}
